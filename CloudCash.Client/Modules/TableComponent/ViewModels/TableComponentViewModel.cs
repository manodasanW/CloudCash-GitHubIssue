using CloudCash.BL.DbAccess;
using CloudCash.BL.DTOs.Sells;
using CloudCash.BL.DTOs.Tables;
using CloudCash.Client.Modules.Table.Messages;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System;
using System.Linq;

namespace CloudCash.Client.Modules.TableComponent.ViewModels
{
    public class TableComponentViewModel : ViewModelBase
    {
        private readonly DbTable _dbtable;
        private readonly DbTableInfo _dbTableInfo;
        private readonly DbTableCategory _dbTableCategory;

        #region Props

        private TableDetailModel _table;
        public TableDetailModel Table
        {
            get => _table;
            set
            {
                _table = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LastSellTime));
            }
        }

        public DateTime LastSellTime => Table.Sells.Count > 0 ? Table.Sells.Max(x => x.DateTime) : DateTime.Now;

        #endregion

        #region Commands
        #endregion

        #region Interface

        public TableComponentViewModel(IMessenger messenger, DbTable dbtable, DbTableInfo dbTableInfo, DbTableCategory dbTableCategory) : base(messenger)
        {
            _dbtable = dbtable;
            _dbTableInfo = dbTableInfo;
            _dbTableCategory = dbTableCategory;

            _messenger.Register<SellAddedToTableMsg>(SellAddedToTable);
        }

        #endregion

        #region Private

        private void LoadOpenedTableData(long id) => Table = _dbtable.GetOpenedTableById(id);

        private void SellAddedToTable(SellAddedToTableMsg obj)
        {
            if (obj.TableId == Table.ID)
                Table.Sells.Add(obj.AddedSell);
        }

        #endregion
    }
}
