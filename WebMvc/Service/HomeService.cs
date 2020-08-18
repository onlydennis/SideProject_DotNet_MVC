using CFData;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewModel;

namespace Service
{
    public class HomeService
    {
        public List<KeyValuesViewModel> ReadData(KeyValuesViewModel query)
        {
            KeyValuesDAO keyValuesDAO = new KeyValuesDAO();

            List<KeyValuesViewModel> result = new List<KeyValuesViewModel>();

            try
            {
                var dbResult = keyValuesDAO.QueryAll(query);

                if (dbResult.Any())
                {
                    result = dbResult.Select(n => new KeyValuesViewModel
                    {
                        Id = n.Id,
                        KeyValueName = n.KeyValueName,
                        Text = n.Text,
                        Value = n.Value,
                        Sort = n.Sort,
                        EnterPriseId = n.EnterPriseId
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return result;
        }

        public bool Update(KeyValuesViewModel data, out string errMsg)
        {
            KeyValuesDAO keyValuesDAO = new KeyValuesDAO();

            errMsg = "";

            try
            {
                var dbModel = keyValuesDAO.FindById(data.Id);

                dbModel.KeyValueName = data.KeyValueName;
                dbModel.Text = data.Text;
                dbModel.Value = data.Value;
                dbModel.Sort = data.Sort;
                dbModel.EnterPriseId = data.EnterPriseId;

                keyValuesDAO.Update(dbModel);
                keyValuesDAO.SaveChange();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }

            return true;
        }

        public bool Create(KeyValuesViewModel data, out string errMsg)
        {
            KeyValuesDAO keyValuesDAO = new KeyValuesDAO();

            errMsg = "";

            try
            {
                KeyValues modelData = new KeyValues();

                modelData.KeyValueName = data.KeyValueName;
                modelData.Text = data.Text;
                modelData.Value = data.Value;
                modelData.Sort = data.Sort;
                modelData.EnterPriseId = data.EnterPriseId;

                keyValuesDAO.Insert(modelData);
                keyValuesDAO.SaveChange();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }

            return true;
        }
    }
}
