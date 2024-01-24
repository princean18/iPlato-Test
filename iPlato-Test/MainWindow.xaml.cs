using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iPlato_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml   
    /// </summary>
    ///      
    public partial class MainWindow : Window
    {
        //private readonly ViewModelData viewModelData;

        public MainWindow()
        {
            InitializeComponent();

            ViewModelData viewModelData = new ViewModelData();

            Test_DBEntities dbEntities = new Test_DBEntities();

            var query = from x in dbEntities.PROFESSIONS
                        select new { x.NAME, x.DOB, x.PROFESSION1 };

            //viewModelData = new ViewModelData
            //{
            //    ProfessionList = new List<ProfessionDataGrid>()
            //    {
            //        new ProfessionDataGrid
            //        {
            //            Name = "Prince",
            //            DOB = "28/05/1994",
            //            Profession = "Developer",
            //        },
            //    }
            //};

            List<ProfessionDataGrid> ProfessionList = query.Select(
                                                        c => new ProfessionDataGrid
                                                        {
                                                            Name = c.NAME,
                                                            DOB = c.DOB,
                                                            Profession = c.PROFESSION1
                                                        }).ToList(); 
            viewModelData = new ViewModelData
            {
                ProfessionList = ProfessionList
            };

            this.DataContext = viewModelData;

        }
    }
}
