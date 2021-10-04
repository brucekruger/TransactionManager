using System;
using System.Transactions;

namespace TransactionManager
{
    public class VolatileRM : IEnlistmentNotification
    {
        private string _whoAmI = string.Empty;
        private int _memberValue = 0;
        private int _oldMemberValue = 0;
        private bool _forceRollBack = false;

        public int MemberValue
        {
            get => this._memberValue;
            set
            {
                Transaction tran = Transaction.Current;
                if(tran != null)
                {
                    Console.WriteLine($"{this._whoAmI}: MemberValue setter - EnlistVolatile");
                    tran.EnlistVolatile(this, EnlistmentOptions.None);
                }
                this._oldMemberValue = this._memberValue;
                this._memberValue = value;
            }
        }

        public VolatileRM(string whoAmI, bool forceRollBack = false)
        {
            this._whoAmI = whoAmI;
            this._forceRollBack = forceRollBack;
        }

        #region IEnlistmentNotification Members

        public void Commit(Enlistment enlistment)
        {
            Console.WriteLine($"{this._whoAmI}: {nameof(Commit)}");

            // Clear out _oldMemberValue
            this._oldMemberValue = 0;
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            Console.WriteLine($"{this._whoAmI}: {nameof(InDoubt)}");
            enlistment.Done();
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            Console.WriteLine($"{this._whoAmI}: {nameof(Prepare)}");
            if (!this._forceRollBack)
            {
                preparingEnlistment.Prepared();
            }
            else
            {
                preparingEnlistment.ForceRollback();
            }
        }

        public void Rollback(Enlistment enlistment)
        {
            Console.WriteLine($"{this._whoAmI}: {nameof(Rollback)}");

            // Restore previous state
            this._memberValue = this._oldMemberValue;
            this._oldMemberValue = 0;
            enlistment.Done();
        }

        #endregion
    }
}
