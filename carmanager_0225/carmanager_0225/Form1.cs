using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace carmanager_0225
{



    public partial class Form1 : Form
    {
       

       
        public Form1()
        {
            InitializeComponent();
           
            try
            {
                textBox_parkingSpot.Text = DataManager.Cars[0].ParkingSpot.ToString();
                textBox_carNumber.Text = DataManager.Cars[0].CarNumber.ToString();
                textBox_driverName.Text = DataManager.Cars[0].DriverName.ToString();
                textBox_phoneNumber.Text = DataManager.Cars[0].PhoneNumber.ToString();


            }
            catch (Exception ex)
            {
                DataManager.PrintLog("초창기 데이터가 없습니다.");
                DataManager.PrintLog(ex.Message);
                DataManager.PrintLog(ex.StackTrace);

            }
            if(DataManager.Cars.Count > 0)
                dataGridView_parkingManager.DataSource = DataManager.Cars;



        }

        private void button1_close (object sender, EventArgs e)
        {

            Close();
        }

        private void button_parkingAdd_Click(object sender, EventArgs e)
        {
            if(textBox_parkingSpot.Text.Trim() =="")
                MessageBox.Show("주차공간을 입력하세요.");
            else if (textBox_carNumber.Text.Trim() =="")
                MessageBox.Show("차량 번호를 입력해주세요.");
            else if (textBox_driverName.Text.Trim() == "")
                MessageBox.Show("차주 이름을 입력해주세요.");
            else if (textBox_phoneNumber.Text.Trim() == "")
                MessageBox.Show("전화번호를 입력해주세요.");
            else
            {
                try
                {

                    //ParkingCar a = new ParkingCar();
                    ////ParkingCar b = a;
                    ////b.ParkingSpot = 10;
                    //ParkingCar c = new ParkingCar();
                    //c.ParkingSpot = a.ParkingSpot;

                    //int aa = 10;
                    //int bb = aa;
                    //bb = 1000;


                    //Clonable
                   
                    ParkingCar car = DataManager.Cars.Single((x) => x.ParkingSpot.ToString() == textBox_parkingSpot.Text);

                    //ParkingCar c;
                    //for (int i = 0; i < DataManager.Cars.Count; i++)
                    //{
                    //    if(DataManager.Cars[i].ParkingSpot.ToString() == textBox_parkingSpot.Text)
                    //    {
                    //        c = DataManager.Cars[i];
                    //        break;
                    //    }

                    //}
                    


                    
                    if(car.CarNumber.Trim() !="")
                    {
                        MessageBox.Show("해당 공간에 이미 차가 있습니다.");
                    }
                    else
                    {
                        car.CarNumber = textBox_carNumber.Text;
                        car.DriverName = textBox_driverName.Text;
                        car.PhoneNumber = textBox_phoneNumber.Text;
                        car.ParkingTime = DateTime.Now;

                        dataGridView_parkingManager.DataSource = null;
                        dataGridView_parkingManager.DataSource = DataManager.Cars;


                        DataManager.Save(textBox_parkingSpot.Text,textBox_carNumber.Text,textBox_driverName.Text,textBox_phoneNumber.Text);
                        
                        string contents = $"주차공간{textBox_parkingSpot.Text}에 {textBox_carNumber.Text}차를 주차했습니다.";

                        WriteLog(contents);


                    }
                }
                catch (Exception ex)
                {
                    string contents = $"주차공간 {textBox_parkingSpot}은(는) 없습니다";
                    MessageBox.Show(contents);
                }
            }
        }

        private void WriteLog(string contents)
        {
            string logContents = $"[{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}]{contents}";
            DataManager.PrintLog(logContents);
            MessageBox.Show(contents);
            listBox_logPrint.Items.Insert(0,logContents);
            
        }

        bool mytest(ParkingCar x)
        {
            return x.ParkingSpot.ToString() == textBox_parkingSpot.Text;
        }


        private void button_parkingRemove_Click(object sender, EventArgs e)
        {
            if (textBox_parkingSpot.Text.Trim() == "")              
                MessageBox.Show("주차공간을 입력하세요.");
            else
            {
                try
                {
                    //ParkingCar car = DataManager.Cars.Single((x) => x.ParkingSpot.ToString() == textBox_parkingSpot.Text);
                    ParkingCar car = DataManager.Cars.Single(mytest);
                    
                    
                    if (car.CarNumber.Trim() == "")
                    {
                        MessageBox.Show("해당 공간에 아직 차가 없습니다.");
                    }
                    else
                    {
                        string oldCar = car.CarNumber;
                        car.CarNumber = "";
                        car.DriverName = "";
                        car.PhoneNumber = "";
                        car.ParkingTime = new DateTime();

                        dataGridView_parkingManager.DataSource = null;
                        dataGridView_parkingManager.DataSource = DataManager.Cars;

                        DataManager.Save(textBox_parkingSpot.Text, "", "", "", true);

                        string contents = $"주차공간 {textBox_parkingSpot.Text}에서 {oldCar}차가 출차했습니다.";
                        WriteLog(contents);

                    }


                }            
                catch (Exception)
                {
                    string contents = $"주차공간{textBox_parkingSpot.Text}은(는)차가 없습니다.";
                    MessageBox.Show(contents);
                }
            }
        }

        private void dataGridView_parkingManager_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ParkingCar car = dataGridView_parkingManager.CurrentRow.DataBoundItem as ParkingCar;
                textBox_parkingSpot.Text = car.ParkingSpot.ToString();
                textBox_carNumber.Text = car.CarNumber;
                textBox_driverName.Text = car.DriverName;
                textBox_phoneNumber.Text = car.PhoneNumber;

            }
            catch (Exception)
            {

                
            }
        }

        private void timer_DisplayNow_Tick(object sender, EventArgs e)
        {
            label_now_time.Text =
              "현재시간 : " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox_parkingSpot_lookUp.Text, out int parkingSpot);

            if(parkingSpot <=0 )
             {
                WriteLog("주차공간번호는 1 이상의 숫자여야 합니다.");
                return;

            }
            DBHelper.selectQuery(parkingSpot);

            if(DBHelper.dt.Rows.Count<1)
            {
                DBHelper.insertQuery(parkingSpot);
                string contents = $"주차공간 {parkingSpot}이/가 추가 되었습니다.";
                WriteLog(contents);
                button_refresh.PerformClick();
            }
            else
            {
                string contents = "해당 주차 공간 이미 존재";
                WriteLog(contents);
            }

        }

        private void button_SelectedLookUp_Click(object sender, EventArgs e)
        {
            try
            {
                int parkingSpot = int.Parse(textBox_parkingSpot_lookUp.Text);
                string ParkingCar = lookUpParkingSpot(parkingSpot);
                if(ParkingCar != "")
                {
                    string contents = $"주차공간 {parkingSpot}에 주차되어 있는 차는 {ParkingCar}입니다";
                    WriteLog(contents);

                }
                else
                {
                    string contents = $"주차공간 {parkingSpot}에 주차되어 있는 차는 없습니다.";
                    WriteLog(contents);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);

            }
        }

        private string lookUpParkingSpot(int parkingSpot)
        {
            string parkedCarNum = "";
            try
            {
                //for(int i =0; i<DataManager.Cars.Count; i++)
                //{
                //    Console.WriteLine("aa");
                //}

                //each other 각각의
                //DataManager.Cars의 길이만큼 반복
                //item은 Cars 안에 들어있는 값들
                var item2 = 10;
                //item2 = "AA";
                var item3 = "으아";
                //item3 = 100;
                foreach (var item in DataManager.Cars)
                {
                    if (item.ParkingSpot == parkingSpot)
                    {
                        parkedCarNum = item.CarNumber;
                        break;
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);


            }
            return parkedCarNum;
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox_parkingSpot_lookUp.Text, out int parkingSpot);

            if (parkingSpot <= 0)
            {
                WriteLog("주차공간번호는 1 이상의 숫자여야 합니다.");
                return;

            }
            DBHelper.selectQuery(parkingSpot);

            if (DBHelper.dt.Rows.Count == 0)
            {
                string contents = "해당 공간 아직 없음";
                WriteLog(contents);
            }
            else
            {
                DBHelper.deleteQuery(parkingSpot);
                string contents = $"주차공간 {parkingSpot}이/가 삭제 되었습니다.";
                WriteLog(contents);
                button_refresh.PerformClick();
            }

        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            DataManager.Load();
            dataGridView_parkingManager.DataSource = null;
            if(DataManager.Cars.Count>0)
                dataGridView_parkingManager.DataSource = DataManager.Cars;
        }

    }
}
