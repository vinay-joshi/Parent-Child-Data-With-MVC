var ObjectState = {
    Unchanged: 0,
    Added: 1,
    Modified: 2,
    Deleted: 3
};

var salesOrderItemMapping = {
    'SalesOrderItems': {
        key: function (salesOrderItem) {
            return ko.utils.unwrapObservable(salesOrderItem.SalesOrderItemId);
        },
        create: function (options) {
            return new SalesOrderItemViewModel(options.data);
        }
    }
};

var dataConverter = function(key, value) {
    if (key == 'RowVersion' && Array.isArray(value)) {
        var str = String.fromCharCode.apply(null, value);
        return btoa(str) ;
    }
    return value;
};

SalesOrderItemViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, salesOrderItemMapping, self);
    self.flagSalesOrderItemAsEdited = function () {
        if (self.ObjectState() != ObjectState.Added) {
            self.ObjectState(ObjectState.Modified);
        }
        return true;
    },
        self.ExtendedPrice = ko.computed(function () {
            console.log((self.Quantity() * self.UnitPrice()).toFixed(2));
            return (self.Quantity() * self.UnitPrice()).toFixed(2);
        });

};


salesOrderViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, salesOrderItemMapping, self);

    self.save = function () {
        console.log(ko.toJSON(self));
        var dataConverted = ko.toJSON(self, dataConverter);
        console.log(dataConverted);
        $.ajax({
            url: "/Sales/Save",
            type: "POST",
            data: ko.mapping.toJS(dataConverted),
            contenttype: "application/json",
            success: function (data) {
                if (data.salesOrderViewModel != null) {
                    console.log(JSON.stringify(data.salesOrderViewModel));
                    ko.mapping.fromJS(data.salesOrderViewModel, {}, self);
                }
                if (data.newLocation != null)
                    window.location = data.newLocation;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest.status == 400) {
                    $('#MessageToClient').text(XMLHttpRequest.responseText);
                } else {
                    $('#MessageToClient').text('Web Server had an error!!!');
                }
            }
        });
    },
        self.flagSalesOrderAsEdited = function () {
            if (self.ObjectState() != ObjectState.Added) {
                console.log("flagSalesOrderAsEdited Executed !!");
                self.ObjectState(ObjectState.Modified);
            }

            return true;
        };

    self.addSalesOrderItem = function () {
        var salesOrderItem = new SalesOrderItemViewModel({ SalesOrderItemId: 0, ProductCode: "", Quantity: 1, UnitPrice: 0, ObjectState: ObjectState.Added });
        self.SalesOrderItems.push(salesOrderItem);
    },
        self.Total = ko.computed(function () {
            var total = 0;
            ko.utils.arrayForEach(self.SalesOrderItems(), function (salesOrderItem) {
                total += parseFloat(salesOrderItem.ExtendedPrice());
            });
            return total.toFixed(2);
        }),
        self.deleteSalesOrderItem = function (salesOrderItem) {
            self.SalesOrderItems.remove(this);

            if (salesOrderItem.SalesOrderItemId() > 0 && self.SalesOrderItemToDelete.indexOf(salesOrderItem.SalesOrderItemId()) == -1)
                self.SalesOrderItemToDelete.push(salesOrderItem.SalesOrderItemId());
        };
};

$("form").validate({
    submitHandler: function (e) {
        
        var modelModel = salesOrderViewModel(e);
        modelModel.save();
    },
    rules: {
        CustomerName: {
            required: true,
            maxlength: 30
        },
        PONumber: {
            maxlength: 10
        },
        ProductCode: {
            required: true,
            maxlength: 15,
            alphaonly: true
        },
        Quantity: {
            required: true,
            digits: true,
            range: [1, 1000000]
        },
        UnitPrice: {
            required: true,
            number: true,
            range: [0, 100000]
        }

    },

    messages: {
        CustomerName: {
            required: "You can not create a sales Order with empty Customer Name.",
            maxlenght: "Customer Name Lenght must be greter then 30"
        },
        ProductCode: {
            alphaonly: "Please enter only Alpha only"
        }
    }
});

$.validator.addMethod("alphaonly", function (value) {
    return /^[A-Za-z]+$/.test(value);
})