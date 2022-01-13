using Flurl.Http;
using ShikkhanobishTeacherApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ShikkhanobishTeacherApp.View_Model
{
    public class AssignTeacherViewModel:BaseViewMode, INotifyPropertyChanged
    {
        List<Topic> asgnList = new List<Topic>();
        public AssignTeacherViewModel()
        {
             GetAssignTeacher();
        }

        #region Methods
        public async Task GetAssignTeacher()
        {
            asgnList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTopic".GetJsonAsync<List<Topic>>();

            assignList = asgnList;
        }
        #endregion

        #region Bindings
        private List<Topic> assignList1;

        public List<Topic> assignList { get => assignList1; set => SetProperty(ref assignList1, value); }
        #endregion
    }
}
