﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Threading;
using log4net;
using log4net.Config;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace StockD
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string url1 = "http://www.goog";
       
        public MainWindow()
        {
            
        }
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))

                yield return day;
        }
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //Configure the ProgressBar
            ProgressBar1.Minimum = 0;
            ProgressBar1.Maximum = short.MaxValue;
            ProgressBar1.Value = 0;

            //Stores the value of the ProgressBar
            double value = 0;

            //Create a new instance of our ProgressBar Delegate that points
            //  to the ProgressBar's SetValue method.
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(ProgressBar1.SetValue);

            //Tight Loop:  Loop until the ProgressBar.Value reaches the max
            do
            {
                value += 10;
                btnStart.IsEnabled = false;
                /*Update the Value of the ProgressBar:
                  1)  Pass the "updatePbDelegate" delegate that points to the ProgressBar1.SetValue method
                  2)  Set the DispatcherPriority to "Background"
                  3)  Pass an Object() Array containing the property to update (ProgressBar.ValueProperty) and the new value */
                Dispatcher.Invoke(updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { System.Windows.Controls.ProgressBar.ValueProperty, value });

           string strYearDir = txtTargetFolder.Text + "\\Downloads";
            string baseurl,filename="";
             DateTime  StartDate, EndDate;

            
                 StartDate = Convert.ToDateTime(dtStartDate.Text);
                 EndDate = Convert.ToDateTime(dtEndDate.Text);
             
            if (!Directory.Exists(strYearDir))
                Directory.CreateDirectory(strYearDir);

            if (Cb_NSE_Sec_List.IsChecked == true)
            {
                value += 10;

                strYearDir = txtTargetFolder.Text + "\\Downloads\\sec_list.csv";
                baseurl="http://www.nseindia.com/content/equities/sec_list.csv";
                downliaddata(strYearDir,baseurl);
            }
            
            if (Cb_NSE_EOD_BhavCopy.IsChecked == true)
            {

                value += 10;
                foreach (DateTime day in EachDay(StartDate, EndDate))
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(day.Month).ToString();
                    strYearDir = txtTargetFolder.Text + "\\Downloads\\cm"+day.Day  + strMonthName +day.Year +"bhav.csv.zip";
                    baseurl = "http://www.nseindia.com/content/historical/EQUITIES/"+day.Year .ToString()+"/" + strMonthName.ToUpper() + "/cm" + day.Day + strMonthName.ToUpper() + day.Year + "bhav.csv.zip";




                    downliaddata(strYearDir, baseurl);
                }
               
            }

            if (chkCombinedReport.IsChecked == true)
            {
                value += 10;
                foreach (DateTime day in EachDay(StartDate, EndDate))
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(day.Month).ToString();
                    strYearDir = txtTargetFolder.Text + "\\Downloads\\combined_report"+formatdate(day)+".zip";
                    baseurl = "http://www.nseindia.com/archives/combine_report/combined_report"+formatdate(day)+".zip";

                    //http://www.nseindia.com/archives/combine_report/combined_report16052013.zip


                    downliaddata(strYearDir, baseurl);
                }
               
            }

            if (Cb_NSE_Delivary_Data_Download.IsChecked == true)
            {
                value += 10;

                foreach (DateTime day in EachDay(StartDate, EndDate))
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(day.Month).ToString();

               
                    //baseurl = "http://www.nseindia.com/content/historical/EQUITIES/" + day.Year.ToString() + "/" + strMonthName.ToUpper() + "/cm" + day.Day + strMonthName.ToUpper() + day.Year + "bhav.csv.zip";

                    strYearDir = txtTargetFolder.Text + "\\Downloads\\MTO_" + formatdate(day) + ".xls";

                    baseurl = " http://nseindia.com/archives/equities/mto/MTO_" + formatdate(day)+".DAT" ;
               



                    downliaddata(strYearDir, baseurl);
                }

            }


            if (Cb_NSE_Index.IsChecked == true)
            {
                value += 10;

                foreach (DateTime day in EachDay(StartDate, EndDate))
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(day.Month).ToString();



                    strYearDir = txtTargetFolder.Text + "\\Downloads\\ind_close_all_" + formatdate(day) + ".csv";

                    baseurl = "http://nseindia.com/content/indices/ind_close_all_" + formatdate(day) + ".csv";



                    downliaddata(strYearDir, baseurl);
                }

            }


            if (Cb_NSE_PR.IsChecked == true)
            {
                value += 10;
                foreach (DateTime day in EachDay(StartDate, EndDate))
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(day.Month).ToString();
                    string date1, year;


                    if (day.Day < 10)
                    {
                        date1 = "0" + day.Day.ToString();
                    }
                    else
                    {
                        date1 = day.Day.ToString();
                    }

                    if (day.Month < 10)
                    {

                        date1 = date1 + "0" + day.Month.ToString();
                    }
                    else
                    {
                        date1 = date1 + day.Month.ToString();
                    }
                    year = day.Year.ToString();

                    string lastTwoChars = year.Substring(year.Length - 2);
                    strYearDir = txtTargetFolder.Text + "\\Downloads\\PR" + date1 + lastTwoChars + ".zip";

                    baseurl = "http://www.nseindia.com/archives/equities/bhavcopy/pr/PR" + date1 + lastTwoChars + ".zip";

                    //http://www.nseindia.com/archives/equities/bhavcopy/pr/PR160513.zip

                    downliaddata(strYearDir, baseurl);
                }

            }
            if (Cb_NSE_Market_Activity.IsChecked == true)
            {
                value += 10;
                foreach (DateTime day in EachDay(StartDate, EndDate))
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(day.Month).ToString();
                    string date1,year;
                    

                    if (day.Day < 10)
                    {
                        date1 = "0" + day.Day.ToString();
                    }
                    else
                    {
                        date1 = day.Day.ToString();
                    }

                    if (day.Month < 10)
                    {

                        date1 = date1 + "0" + day.Month.ToString();
                    }
                    else
                    {
                        date1 = date1 + day.Month.ToString();
                    }
                    year = day.Year.ToString();

                    string lastTwoChars = year.Substring(year.Length - 2);
                    strYearDir = txtTargetFolder.Text + "\\Downloads\\MA" + date1 + lastTwoChars  + ".csv";

                    baseurl = "http://www.nseindia.com/archives/equities/mkt/MA" + date1 + lastTwoChars +".csv";

                //http://www.nseindia.com/archives/equities/mkt/MA160513.csv

                    downliaddata(strYearDir, baseurl);
                }

            }

            if (Cb_NSE_Bulk_Deal.IsChecked == true)
            {
                value += 10;

                foreach (DateTime day in EachDay(StartDate, EndDate))
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(day.Month).ToString();
                    string day1,month, year;


                    if (day.Day < 10)
                    {
                        day1  = "0" + day.Day.ToString()+"-";
                    }
                    else
                    {
                        day1 =  day.Day.ToString() + "-";

                    }

                    if (day.Month < 10)
                    {

                        month  = "0" + day.Month.ToString()+"-";
                    }
                    else
                    {
                        month =  day.Month.ToString() + "-";

                    }
                  

                    string date1=day1 + month + day.Year ;
                    strYearDir = txtTargetFolder.Text + "\\Downloads\\" + date1 +"-TO-"+date1+"_bulk.csv";

                    baseurl = "http://www.nseindia.com/content/equities/bulkdeals/datafiles/" + date1 + "-TO-" + date1 + "_bulk.csv";

              // baseurl=" http://www.nseindia.com/content/equities/bulkdeals/datafiles/06-05-2013-TO-09-05-2013_bulk.csv";

                    downliaddata(strYearDir, baseurl);
                }

            }

            if (Cb_NSE_Block_Deal.IsChecked == true)
            {
                value += 10;

                foreach (DateTime day in EachDay(StartDate, EndDate))
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(day.Month).ToString();
                    string day1, month, year;


                    if (day.Day < 10)
                    {
                        day1 = "0" + day.Day.ToString() + "-";
                    }
                    else
                    {
                        day1 = day.Day.ToString() + "-";

                    }

                    if (day.Month < 10)
                    {

                        month = "0" + day.Month.ToString() + "-";
                    }
                    else
                    {
                        month = day.Month.ToString() + "-";

                    }


                    string date1 = day1 + month + day.Year;
                    strYearDir = txtTargetFolder.Text + "\\Downloads\\" + date1 + "-TO-" + date1 + "_block.csv";

                    baseurl = "http://www.nseindia.com/content/equities/bulkdeals/datafiles/" + date1 + "-TO-" + date1 + "_block.csv";

                    // baseurl=" http://www.nseindia.com/content/equities/bulkdeals/datafiles/09-05-2013-TO-09-05-2013_block.csv

                    downliaddata(strYearDir, baseurl);
                }

            }


            if (Cb_NSE_India_Vix.IsChecked == true)
            {
                value += 10;
                foreach (DateTime day in EachDay(StartDate, EndDate))
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(day.Month).ToString();
                    string day1, month, year;


                    if (day.Day < 10)
                    {
                        day1 = "0" + day.Day.ToString() ;
                    }
                    else
                    {
                        day1 = day.Day.ToString() ;

                    }

                    string date1 = day1 +"-"+strMonthName +"-"+ day.Year;
                   

                    strYearDir = txtTargetFolder.Text + "\\Downloads\\" + date1 + "_" + date1 + ".csv";
                    baseurl = "http://www.nseindia.com/content/vix/histdata/hist_india_vix_"+ date1 + "_" + date1 + ".csv";

                    // baseurl=" http://www.nseindia.com/content/vix/histdata/hist_india_vix_06-May-2013_06-May-2013.csv

                    downliaddata(strYearDir, baseurl);
                }

            }

            
                 if (Cb_NSE_Vix.IsChecked == true)
            {
                value += 10;
                foreach (DateTime day in EachDay(StartDate, EndDate))
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(day.Month).ToString();
                    string day1, month, year;


                    if (day.Day < 10)
                    {
                        day1 = "0" + day.Day.ToString();
                    }
                    else
                    {
                        day1 = day.Day.ToString();

                    }

                    string date1 = day1 + "-" + strMonthName + "-" + day.Year;


                    strYearDir = txtTargetFolder.Text + "\\Downloads\\" + date1 + "_" + date1 + ".csv";
                    baseurl = "http://www.nseindia.com/content/vix/histdata/hist_india_vix_" + date1 + "_" + date1 + ".csv";

                    // baseurl=" http://www.nseindia.com/content/vix/histdata/hist_india_vix_06-May-2013_06-May-2013.csv

                    downliaddata(strYearDir, baseurl);
                }

            }

         if (Cb_BSE_CASH_MARKET.IsChecked == true)
            {
                value += 10;
                foreach (DateTime day in EachDay(StartDate, EndDate))
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(day.Month).ToString();
                    string day1, month, year,date1;


                    if (day.Day < 10)
                    {
                        date1 = "0" + day.Day.ToString();
                    }
                    else
                    {
                        date1 = day.Day.ToString();
                    }

                    if (day.Month < 10)
                    {

                        date1 = date1 + "0" + day.Month.ToString();
                    }
                    else
                    {
                        date1 = date1 + day.Month.ToString();
                    }
                    year = day.Year.ToString();

                    string lastTwoChars = year.Substring(year.Length - 2);
                    strYearDir = txtTargetFolder.Text + "\\Downloads\\eq" + date1 + lastTwoChars + "_csv.zip";
                    baseurl = " http://www.bseindia.com/download/BhavCopy/Equity/eq" + date1 + lastTwoChars + "_csv.zip";

                  
                    downliaddata(strYearDir, baseurl);
                }

            }

         if (BSE_Delivary_Data.IsChecked == true)
         {
             value += 10;
             foreach (DateTime day in EachDay(StartDate, EndDate))
             {
                 System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                 string strMonthName = mfi.GetMonthName(day.Month).ToString();
                 string day1, month, year, date1;


                 if (day.Day < 10)
                 {
                     date1 = "0" + day.Day.ToString();
                 }
                 else
                 {
                     date1 = day.Day.ToString();
                 }

                
                 year = day.Year.ToString();

                 string lastTwoChars = year.Substring(year.Length - 2);
                 strYearDir = txtTargetFolder.Text + "\\Downloads\\SCBSEALL" + date1 + lastTwoChars + ".zip";
                 baseurl = "http://www.bseindia.com/BSEDATA/gross/" + day.Year + "/SCBSEALL" + date1 + lastTwoChars + ".zip";

                 downliaddata(strYearDir, baseurl);
             }

         }

         if (Cb_BSE_Equity_Futures.IsChecked == true)
         {
             value += 10;

             foreach (DateTime day in EachDay(StartDate, EndDate))
             {
                 System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                 string strMonthName = mfi.GetMonthName(day.Month).ToString();
                 string day1, month, year, date1,date2,datetoselect;


                 if (day.Day < 10)
                 {
                     date1 = "0" + day.Day.ToString();
                 }
                 else
                 {
                     date1 = day.Day.ToString();
                 }
                 if (day.Month < 10)
                 {

                     date2 =  "0" + day.Month.ToString();
                 }
                 else
                 {
                     date2 =  day.Month.ToString();
                 }

                 year = day.Year.ToString();
                 string lastTwoChars = year.Substring(year.Length - 2);
                 datetoselect = date1 + "-" + date2 + "-" + lastTwoChars;

                 strYearDir = txtTargetFolder.Text + "\\Downloads\\bhavcopy" + datetoselect + ".zip";
                 baseurl = "http://www.bseindia.com/download/Bhavcopy/Derivative/bhavcopy" +datetoselect + ".zip";
                 //http://www.bseindia.com/download/Bhavcopy/Derivative/bhavcopy23-11-12.zip
                 downliaddata(strYearDir, baseurl);
             }

         }

         if (BSE_Block.IsChecked == true)
         {
             value += 10;
             foreach (DateTime day in EachDay(StartDate, EndDate))
             {
                 System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                 string strMonthName = mfi.GetMonthName(day.Month).ToString();
                 string day1, month, year, date1, date2, datetoselect;


                 if (day.Day < 10)
                 {
                     date1 = "0" + day.Day.ToString();
                 }
                 else
                 {
                     date1 = day.Day.ToString();
                 }
                 if (day.Month < 10)
                 {

                     date2 = "0" + day.Month.ToString();
                 }
                 else
                 {
                     date2 = day.Month.ToString();
                 }

                 year = day.Year.ToString();
                 string lastTwoChars = year.Substring(year.Length - 2);
                 datetoselect = date1 + "-" + date2 + "-" + lastTwoChars;
                 strMonthName = strMonthName.Substring(0, 3);
                 strYearDir = txtTargetFolder.Text + "\\Downloads\\Block_" + day.Day  + ".csv";
                 baseurl = "http://www.bseindia.com/stockinfo/BulkBlockFiles/Block_" + date1 + strMonthName + day.Year + ".csv";
                 //http://www.bseindia.com/stockinfo/BulkBlockFiles/Block_26Dec2012.csv
                 downliaddata(strYearDir, baseurl);
             }

         }
             
         if (BSE_Index.IsChecked == true)
         {
             value += 10;
             strYearDir =  txtTargetFolder.Text + "\\Downloads\\Bse";
             if (!Directory.Exists(strYearDir))
                 Directory.CreateDirectory(strYearDir);

             foreach (DateTime day in EachDay(StartDate, EndDate))
             {
                 System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                 string strMonthName = mfi.GetMonthName(day.Month).ToString();
                 string day1, month, year, date1, date2, datetoselect;


                 if (day.Day < 10)
                 {
                     date1 = "0" + day.Day.ToString();
                 }
                 else
                 {
                     date1 = day.Day.ToString();
                 }
                 if (day.Month < 10)
                 {

                     date2 = "0" + day.Month.ToString();
                 }
                 else
                 {
                     date2 = day.Month.ToString();
                 }

                 year = day.Year.ToString();
                 string lastTwoChars = year.Substring(year.Length - 2);
                 datetoselect = date2 + "/" + date1 + "/" +day.Year ;
                 filename=day.Day.ToString() ;
                    //BSE30
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSE30.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSE30%20%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //MIDCAP
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\MIDCAP.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=MIDCAP%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //SMLCAP
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\SMLCAP.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=SMLCAP%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);
                     //BSE100
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSE100.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSE100%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);
                   //BSE200
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSE200.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSE200%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //BSE500
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSE500.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSE500%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //BSE500
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSE500.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSE500%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //AUTO
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\AUTO.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=AUTO%20%20%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //BANKEX
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BANKEX.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BANKEX%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //BSECD
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSECD.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSECD%20%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                  //BSECG
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSECG.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSECG%20%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);


                     //BSEFMCG
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSEFMCG.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSEFMCG&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //BSEHC
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSEHC.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSEHC%20%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);


                     //BSEIT
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSEIT.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSEIT%20%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //METAL
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\METAL.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=METAL%20%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //OILGAS
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\OILGAS.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=OILGAS%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);


                     //POWER
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\POWER.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=POWER%20%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //BSEPSU
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSEPSU.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSEPSU%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //REALTY
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\REALTY.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=REALTY%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //TECK
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\TECK.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=TECK%20%20%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);


                     //DOL
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\DOL.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=DOL30%20%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //DOL100
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\DOL100.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=DOL100%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //DOL200
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\DOL200.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=DOL200%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //SHA50
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\SHA50.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=SHA50%20%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //GREENX
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\GREENX.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=GREENX%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);
                     //BSEIPO
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\BSEIPO.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=BSEIPO%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //CARBON
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\CARBON.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=CARBON%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);

                     //SMEIPO
                     strYearDir = txtTargetFolder.Text + "\\Downloads\\Bse\\SMEIPO.csv";
                     baseurl = "http://www.bseindia.com/stockinfo/indices_main_excel.aspx?ind=SMEIPO%20&fromDate=" + datetoselect + "&toDate=" + datetoselect + "&DMY=D";
                     downliaddata(strYearDir, baseurl);


                     string[] csvFileNames = Directory.GetFiles(@"C:\Documents and Settings\maheshwar\My Documents\data\Downloads\bse", "*.csv");

                     JoinCsvFiles(csvFileNames, @"C:\Documents and Settings\maheshwar\My Documents\data\Downloads\BSEIndex"+day.Day +".csv");

                
             }
             

         }


         
                value += 10;
                ProgressBar1.Value = ProgressBar1.Maximum;
         }
         while (ProgressBar1.Value != ProgressBar1.Maximum);

         btnStart.IsEnabled =true ;
         System.Windows.Forms.MessageBox.Show("Download Completed Please See Log File In c:\\Temp");

        }
        private static void JoinCsvFiles(string[] csvFileNames, string outputDestinationPath)
        {
            StringBuilder sb = new StringBuilder();

            bool columnHeadersRead = false;

            foreach (string csvFileName in csvFileNames)
            {
                TextReader tr = new StreamReader(csvFileName);

                string columnHeaders = tr.ReadLine();

                // Skip appending column headers if already appended
                if (!columnHeadersRead)
                {
                    sb.AppendLine(columnHeaders);
                    columnHeadersRead = true;
                }

                sb.AppendLine(tr.ReadToEnd());

                
            }
            
            File.WriteAllText(outputDestinationPath, sb.ToString());
            
          
        }

        private string formatdate(DateTime day)
        {
            string date1;
            if (day.Day < 10)
            {
                date1 = "0" + day.Day.ToString();
            }
            else
            {
                date1 = day.Day.ToString();
            }

            if (day.Month < 10)
            {

                date1 = date1 + "0" + day.Month.ToString();
            }
            else
            {
                date1 = date1 + day.Month.ToString();
            }
            date1 = date1 + day.Year;
            return date1;
        }
        private void downliaddata(string path,string url)
        {
             WebClient Client = new WebClient();

                    try
                    {

                        //If Data is Not Present For Date Then  Exception Occure And It Get Added Into List Box  
                       // Client.DownloadFile("http://www.mcx-sx.com/downloads/daily/EquityDownloads/Market%20Statistics%20Report_" + date1 + ".csv.", File_path);

                        log4net.Config.XmlConfigurator.Configure();
                        ILog log = LogManager.GetLogger(typeof(MainWindow));
                        log.Debug(url + "Download Started at " + DateTime.Now.ToString("HH:mm:ss tt"));

                        Client.Headers.Add("Accept", "application/zip");
                        Client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                        Client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1");
                        Client.DownloadFile(url, path );


                        log.Debug(url + "Download Completed at " + DateTime.Now.ToString("HH:mm:ss tt"));

                        //string clientHeader = "DATE" + "," + "TICKER" + " " + "," + "NAME" + "," + " " + "," + " " + "," + "OPEN" + "," + "HIGH" + "," + "LOW" + "," + "CLOSE" + "," + "VOLUME" + "," + "OPENINT" + Environment.NewLine;

                        //Format_Header(File_path, clientHeader);
                    }
                    catch (Exception ex)
                    {
                        if ((ex.ToString().Contains("404")) || (ex.ToString().Contains("400")))
                        {
                            log4net.Config.XmlConfigurator.Configure();
                            ILog log = LogManager.GetLogger(typeof(MainWindow));
                            log.Warn("Data Not Found For " +url );
                           
                        }
                    }
                   

                }

        


        

       
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
           // Lbl_internet.Content = DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
            url1 = "http://www.google.com";
            Check_internet_connetion(url1);

        }
        private void Check_internet_connetion(string url)
        {
            //Check Internet Connection Is Present Or Not
            DispatcherTimer DispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();

            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
                System.Net.WebResponse myResponse = myRequest.GetResponse();
                Net_Connection.Fill = new SolidColorBrush(Colors.Green);
                //Connection is ok time stop
                DispatcherTimer1.Stop();
            }
            catch (System.Net.WebException)
            {
                Net_Connection.Fill = new SolidColorBrush(Colors.Red);
                DispatcherTimer1.Tick += new EventHandler(dispatcherTimer_Tick);
                DispatcherTimer1.Interval = new TimeSpan(0, 0, 10);
                DispatcherTimer1.Start();
            }
        }

        private void wMain_Loaded(object sender, RoutedEventArgs e)
        {

            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger(typeof(MainWindow));
            log.Debug("Application Strated Successfully");
            
            BinaryFormatter bf = new BinaryFormatter();
            //Use For Serialization Data Get Save In Fileio.txt file 


            if (File.Exists(@"C:\Fileio.txt"))
            {

                FileStream fs = new FileStream(@"C:\Fileio.txt", FileMode.Open, FileAccess.Read);
                target1 t1 = (target1)bf.Deserialize(fs);
                txtTargetFolder.Text = t1.target;
                 dtStartDate.Text = t1.fromdate.ToShortDateString();
                 dtEndDate.Text = t1.todate.ToShortDateString();
                dtEndDate.Text = DateTime.Today.ToShortDateString();
                dtStartDate.Text = DateTime.Today.ToShortDateString();
                Cb_BSE_CASH_MARKET.IsChecked = t1.Cb_BSE_CASH_MARKET;
                BSE_Delivary_Data.IsChecked = t1.BSE_Delivary_Data;
                BSE_Block.IsChecked = t1.BSE_Block;


                



                Cb_BSE_Equity_Futures.IsChecked = t1.Cb_BSE_Equity_Futures;
                BSE_Index.IsChecked = t1.BSE_Index;

                ChkBseFo.IsChecked = t1.ChkBseFo;


                Cb_NSE_CASH_MARKET.IsChecked = t1.Cb_NSE_CASH_MARKET;
                Cb_NSE_EOD_BhavCopy.IsChecked = t1.Cb_NSE_EOD_BhavCopy;
                chkEquity.IsChecked = t1.chkEquity;
                Cb_NSE_Forex_Options.IsChecked = t1.Cb_NSE_Forex_Options;
                Cb_NSE_SME.IsChecked = t1.Cb_NSE_SME;
                Cb_NSE_ETF.IsChecked = t1.Cb_NSE_ETF;
                Cb_NSE_Index.IsChecked = t1.Cb_NSE_Index;
                Cb_Reports.IsChecked = t1.Cb_Reports;
                chkCombinedReport.IsChecked = t1.chkCombinedReport;
                chkNseForex.IsChecked = t1.chkNseForex;
                chkNseNcdex.IsChecked = t1.chkNseNcdex;


                Cb_NSE_Sec_List.IsChecked = t1.Cb_NSE_Sec_List;
                Cb_NSE_Delivary_Data_Download.IsChecked = t1.Cb_NSE_Delivary_Data_Download;

                Cb_NSE_Market_Activity.IsChecked = t1.Cb_NSE_Market_Activity;

                Cb_NSE_PR.IsChecked = t1.Cb_NSE_PR;
                Cb_NSE_Bulk_Deal.IsChecked = t1.Cb_NSE_Bulk_Deal;
                Cb_NSE_Block_Deal.IsChecked = t1.Cb_NSE_Block_Deal;
                Cb_NSE_India_Vix.IsChecked = t1.Cb_NSE_India_Vix;
                Cb_NSE_Vix.IsChecked = t1.Cb_NSE_Vix;





                MCXSX_Forex_Future.IsChecked = t1.MCXSX_Forex_Future;
                MCXSX_Equity_Futures.IsChecked = t1.MCXSX_Equity_Futures;
                MCXCommodity_Futures.IsChecked = t1.MCXCommodity_Futures;
                MCXSX_Equity_Options.IsChecked = t1.MCXSX_Equity_Options;
                MCXSXForex_Options.IsChecked = t1.MCXSXForex_Options;
                National_Spot_Exchange.IsChecked = t1.National_Spot_Exchange;
                MCXSX_Equity_Indices.IsChecked = t1.MCXSX_Equity_Indices;
                MCX_Index.IsChecked = t1.MCX_Index;


                chkYahooEOD.IsChecked = t1.chkYahooEOD;
                ChkYahooIEOD1.IsChecked = t1.ChkYahooIEOD1;
                chkYahooFundamental.IsChecked = t1.chkYahooFundamental;
                ChkYahooIEOD5.IsChecked = t1.ChkYahooIEOD5;
                Cb_Yahoo_Realtime.IsChecked = t1.Cb_Yahoo_Realtime;

                ChkGoogleEOD.IsChecked = t1.ChkGoogleEOD;
                ChkGoogleIEOD.IsChecked = t1.ChkGoogleIEOD;
                Cb_MCX_Google_IEOD_5min.IsChecked = t1.Cb_MCX_Google_IEOD_5min;


                Cb_Corporate_Events.IsChecked = t1.Cb_Corporate_Events;
                Cb_Board_Message.IsChecked = t1.Cb_Board_Message;
                Cb_Delete_all_events.IsChecked = t1.Cb_Delete_all_events;



                fs.Close();


            }
            else
            {
                dtStartDate.Text = DateTime.Today.Date.ToString();
                dtEndDate.Text = DateTime.Today.Date.ToString();
                textBox1.Text = "";
            }
           
            Check_internet_connetion(url1);
        }

        private void wMain_Closed(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger(typeof(MainWindow));
            log.Debug("Application Close ");
            savechanges();
           
        }
        private void savechanges()
        {
            //if (dtStartDate.Text.ToString() == "")
            //{

            //}
            //else
            {
                target1 t = new target1();
               // t.fromdate = Convert.ToDateTime(dtStartDate.Text);
               // t.todate = Convert.ToDateTime(dtEndDate.Text);
                t.target = txtTargetFolder.Text;
                
                t.Cb_BSE_CASH_MARKET=Cb_BSE_CASH_MARKET.IsChecked.Value ;
t.Cb_BSE_Equity_Futures=Cb_BSE_Equity_Futures.IsChecked.Value;
t.ChkBseFo=ChkBseFo.IsChecked.Value;
                

t.Cb_NSE_CASH_MARKET=Cb_NSE_CASH_MARKET.IsChecked.Value;
t.Cb_NSE_EOD_BhavCopy=Cb_NSE_EOD_BhavCopy.IsChecked.Value;
t.chkEquity=chkEquity.IsChecked.Value;
t.Cb_NSE_Forex_Options=Cb_NSE_Forex_Options.IsChecked.Value;
t.Cb_NSE_SME=Cb_NSE_SME.IsChecked.Value;
t.Cb_NSE_ETF = Cb_NSE_ETF.IsChecked.Value;
t.Cb_NSE_Index= Cb_NSE_Index.IsChecked.Value;
t.Cb_Reports= Cb_Reports.IsChecked.Value;
t.chkCombinedReport= chkCombinedReport.IsChecked.Value;
t.chkNseForex= chkNseForex.IsChecked.Value;
t.chkNseNcdex= chkNseNcdex.IsChecked.Value;
     


t.MCXSX_Forex_Future= MCXSX_Forex_Future.IsChecked.Value;
t.MCXSX_Equity_Futures = MCXSX_Equity_Futures.IsChecked.Value;
t.MCXCommodity_Futures= MCXCommodity_Futures.IsChecked.Value;
t.MCXSX_Equity_Options= MCXSX_Equity_Options.IsChecked.Value;
t.MCXSXForex_Options= MCXSXForex_Options.IsChecked.Value;
t.National_Spot_Exchange= National_Spot_Exchange.IsChecked.Value;
t.MCXSX_Equity_Indices= MCXSX_Equity_Indices.IsChecked.Value;
t.MCX_Index=  MCX_Index.IsChecked.Value;


t.chkYahooEOD= chkYahooEOD.IsChecked.Value;
t.ChkYahooIEOD1= ChkYahooIEOD1.IsChecked.Value;
t.chkYahooFundamental= chkYahooFundamental.IsChecked.Value;
t.ChkYahooIEOD5= ChkYahooIEOD5.IsChecked.Value;
t.Cb_Yahoo_Realtime= Cb_Yahoo_Realtime.IsChecked.Value;

t.ChkGoogleEOD= ChkGoogleEOD.IsChecked.Value;
t.ChkGoogleIEOD= ChkGoogleIEOD.IsChecked.Value;
t.Cb_MCX_Google_IEOD_5min= Cb_MCX_Google_IEOD_5min.IsChecked.Value;


t.Cb_Corporate_Events= Cb_Corporate_Events.IsChecked.Value;
t.Cb_Board_Message= Cb_Board_Message.IsChecked.Value;
t.Cb_Delete_all_events= Cb_Delete_all_events.IsChecked.Value;




t.Cb_NSE_Sec_List = Cb_NSE_Sec_List.IsChecked.Value;
t.Cb_NSE_Delivary_Data_Download = Cb_NSE_Delivary_Data_Download.IsChecked.Value;

t.Cb_NSE_Market_Activity = Cb_NSE_Market_Activity.IsChecked.Value;
t.Cb_NSE_PR = Cb_NSE_PR.IsChecked.Value;
t.Cb_NSE_Bulk_Deal = Cb_NSE_Bulk_Deal.IsChecked.Value;
t.Cb_NSE_Block_Deal = Cb_NSE_Block_Deal.IsChecked.Value; 
t.Cb_NSE_India_Vix = Cb_NSE_India_Vix.IsChecked.Value;
t.Cb_NSE_Vix = Cb_NSE_Vix.IsChecked.Value;
t.BSE_Delivary_Data = BSE_Delivary_Data.IsChecked.Value;
t.BSE_Index = BSE_Index.IsChecked.Value;
t.BSE_Block = BSE_Block.IsChecked.Value; 


                
                
                
                

                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(@"C:\Fileio.txt", FileMode.Create, FileAccess.Write);
                bf.Serialize(fs, t);

                fs.Close();
            }
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
           
           
        }

        private void tabItem2_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            savechanges();
        }

        private void btnTarget_Click(object sender, RoutedEventArgs e)
        {
            var Open_Folder = new System.Windows.Forms.FolderBrowserDialog();
            if (Open_Folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Target_Folder_Path = Open_Folder.SelectedPath;


                txtTargetFolder.Text = Target_Folder_Path;
            }

        }

        private void image2_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }

       

     }
    [Serializable]
    public class target1
    {
        public string target;
        public DateTime fromdate;
        public DateTime todate;
        public bool  checkboxevent;


        public bool Cb_NSE_Sec_List;
        public bool Cb_NSE_Market_Activity;
        public bool Cb_NSE_PR;
        public bool Cb_NSE_Bulk_Deal;
        public bool Cb_NSE_Block_Deal;
        public bool Cb_NSE_India_Vix;
        public bool Cb_NSE_Vix;
        public bool BSE_Delivary_Data;
        public bool BSE_Index;

       public bool Cb_BSE_CASH_MARKET;
public bool Cb_BSE_Equity_Futures;
public bool ChkBSEEquity;
public bool ChkBseFo;
 public bool Cb_NSE_Delivary_Data_Download;
 public bool BSE_Block;


public bool Cb_NSE_CASH_MARKET;
public bool Cb_NSE_EOD_BhavCopy;
public bool chkEquity;
public bool Cb_NSE_Forex_Options;
public bool Cb_NSE_SME;
public bool Cb_NSE_ETF;
public bool Cb_NSE_Index;
public bool Cb_Reports;
public bool chkCombinedReport;
public bool chkNseForex;
public bool chkNseNcdex;



public bool MCXSX_Forex_Future;
public bool MCXSX_Equity_Futures;
public bool MCXCommodity_Futures;
public bool MCXSX_Equity_Options;
public bool MCXSXForex_Options;
public bool National_Spot_Exchange;
public bool MCXSX_Equity_Indices;
public bool MCX_Index;


public bool chkYahooEOD;
public bool ChkYahooIEOD1;
public bool chkYahooFundamental;
public bool ChkYahooIEOD5;
public bool Cb_Yahoo_Realtime;

public bool ChkGoogleEOD;
public bool ChkGoogleIEOD;
public bool Cb_MCX_Google_IEOD_5min;


public bool Cb_Corporate_Events;
public bool Cb_Board_Message;
public bool Cb_Delete_all_events;
    }
}
