using System.Data.Entity;
using MVCParentClient.Model;
using MVCParentClient.Model.Interface;

namespace MVCParentClient.DataLayer.Helpers
{
    public static class Helpers
    {
        public static EntityState ConvertState(ObjectState objectState)
        {
            switch (objectState)
            {
                case ObjectState.Added :
                    return EntityState.Added;
                case ObjectState.Modified:
                    return EntityState.Modified;
                case ObjectState.Deleted :
                    return EntityState.Deleted;
                default :
                    return EntityState.Unchanged;
            }
        }

        public static void ApplyStateChanges(this DbContext context)
        {
            foreach (var dbEntityEntry in context.ChangeTracker.Entries<IObjectWithState>())
            {
                IObjectWithState stateInfo = dbEntityEntry.Entity;
                dbEntityEntry.State = ConvertState(stateInfo.ObjectState);
            }
        }
    }
}
