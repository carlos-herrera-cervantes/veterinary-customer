using System.Collections.Generic;
using Newtonsoft.Json;

namespace VeterinaryCustomer.Web.Types
{
    public class Pagination<T> where T : class
    {
        #region snippet_Properties
        
        [JsonProperty("previous")]
        public int Previous { get; set; }

        [JsonProperty("next")]
        public int Next { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("data")]
        public IEnumerable<T> Data { get; set; }
        
        #endregion

        #region snippet_ActionMethods

        public Pagination<T> CalculatePagination(Pager pager)
        {
            var (page, pageSize) = pager;
            var current = page == 0 ? 1 * pageSize : page * pageSize;
            
            Next = current < Total ? page + 1 : 0;
            Previous = current > pageSize ? page - 1 : 0;

            return this;
        }

        #endregion
    }
}
