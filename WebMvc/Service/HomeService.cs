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
    }
}
