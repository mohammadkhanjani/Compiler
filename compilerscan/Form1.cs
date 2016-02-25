using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace compilerscan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            value();
            InitializeComponent();
            label3.Hide();
            label2.Hide();
            label4.Hide();
            listBox4.Hide();
        }
        




        //*****************************lexer***********************************************

        string[,] saved_tokent = new string[10000, 3];

        string[] reserved = new string[21] { "mot", "barname", "shoro", "payan", "vaghe", "sahih", "boly", "reshte", "array", "agar", "angah", "vagarna", "dorost", "ghalat", "baraye", "tohy", "tavaghty", "anjam","tabe","benvis","bekhan"};
             
        
        //موجود در اکسل همراه این پروژه می باشد dfa جدول
        int[,] dfa_of_project;
        
        char[] alamat;

        public void value()
            {
                dfa_of_project = new int[35, 65] {
                    {9,	18,	8,	8,	8,	19,	23,	8,	17,	17,	17,	17,	29,	26,	32,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	3,	3,	3,	3,	3,	3,	3,	3,	3,	3,	17,	17,	17,	17,	12,	15,	-3	,8	,8	,33	,-1	,-1	,-1	,-1},/* 0 */
                    {2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	1},/* 1 */
                    {-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2},/* 2 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,5	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,3	,3	,3	,3	,3	,3	,3	,3	,3	,3	,-1	,-1	,-1	,-1	,4	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 3 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,5	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,4	,4	,4	,4	,4	,4	,4	,4	,4	,4	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 4 */
                    {7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,6	,6	,6	,6	,6	,6	,6	,6	,6	,6	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7	,7},/* 5 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,6	,6	,6	,6	,6	,6	,6	,6	,6	,6	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 6 */
                    {-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2},/* 7 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 8 */
                    {11	,10	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11	,11},/* 9 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 10 */
                    {-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2},/* 11 */
                    {14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,14	,13	,14	,14	,14	,14	,14	,14	,14	,14	,14},/* 12 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 13 */
                    {-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2},/* 14 */
                    {15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,15	,16	,15	,15	,15	,15	,15	,15	,15},/* 15 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 16 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 17 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 18 */
                    {21	,20	,21	,21	,21	,21	,22	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21	,21},/* 19 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 20 */
                    {-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2},/* 21 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 22 */
                    {25	,24	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25	,25},/* 23 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 24 */
                    {-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2},/* 25 */
                    {28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,27	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28	,28},/* 26 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 27 */
                    {-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2},/* 28 */
                    {31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,30	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31	,31},/* 29 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 30 */
                    {-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2	,-2},/* 31 */
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/* 32 */
                    {33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,33	,34	,33	,33	,33	,33},/*33*/
                    {-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1	,-1},/*34*/
                    };



                alamat = new char[65] { ':', '=', ';', '(', ')', '<', '>', ',', '+', '-', '/', '*', '|', '&', '!', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '%', '^', '\\', '@', '.', '{', '}', '[', ']', '\'', ' ', '\n', '$', '_' };
            }

            
       
        public  struct  tokening
       {
          public string first;
           public string second;
       }


        public  tokening tok;
        public bool ok1=false;
        public int q = 0;
        int y = 0; 


    private void showtoken()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            for (int i = 0; i <= y; i++)
            {
                if (saved_tokent[i, 0] != null)
                {
                    listBox1.Items.Add(i.ToString() + ">  " + saved_tokent[i, 0].ToString());
                    listBox2.Items.Add(i.ToString() + ">  " + saved_tokent[i, 1].ToString());

                }
            }
        }
  

    private void token()
        {
            int n, k=0, j=0, m=0,a=0; 
            int current_state = 0; bool ok=false;
            char ch, rch;
           string c,s1,s3,s2=null;
            Array.Clear(saved_tokent, 0, 10000);
            n = richTextBox1.TextLength;
            s1 = richTextBox1.Text.ToString().ToLower();
            for (a = 0; a < n; a++)
            {
                ok = false;
                for (m = 0; m < 65; m++)
                    if (s1[a] == alamat[m])
                    {
                        ok = true;
                        break;
                    }
                if (ok == false)
                {
                    MessageBox.Show("error in type character", s1[a].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else s2 += s1[a];
             }
            s2=s2+"\n$";
            y = 0;
            m = 0;
            while (k!=s2.Length)
            {
                c = null;
                current_state = 0;
                m++;
                int e = 0;
                    while ((current_state >= 0) && (s2.Length != k))
                    {
                        if (j >= 65) j--;
                        if (j < 0) j++;
                        if (dfa_of_project[current_state, j] == -2) { e = current_state; break; }
                        else
                        {
                            ch = s2[k];
                            c += ch;
                            k++;
                        }
                        
                        for (j = 0; j < 65; ++j)
                            if (ch == alamat[j])
                            {
                                e = current_state;
                                current_state = dfa_of_project[current_state, j];
                                break;
                            }
                       
                     }
                    a=k;
                          if((current_state==-3)&&(e==0))
                          {
                            s3 = c;
                            MessageBox.Show(s3.ToString());
                          }
                         

                    switch (e)
                    {
                        
                        case 2:
                            rch = c[(c.Length)-1];
                              if ((rch != ' ') && (rch != '\n')&&(rch!='$'))
                               k--;  
                                  s3 = c.Substring(0, (c.Length)-1);
                                  saved_tokent[m, 0] = s3;
                                  saved_tokent[m, 1] = "ID      " + k.ToString();
                                  saved_tokent[m, 2] = "ID";
                            break;
                        case 3:
                            k--;
                                c = c.Substring(0, (c.Length) - 1); 
                                saved_tokent[m, 0] = c;
                                saved_tokent[m, 1] = "sahih   " + k.ToString();
                                saved_tokent[m, 2] = "sah";
                            break;
                        case 4:
                        case 6:
                            k--;
                                c = c.Substring(0, (c.Length) - 1); 
                            saved_tokent[m, 0] = c;
                            saved_tokent[m, 1] = "vaghe   " + k.ToString();
                            saved_tokent[m, 2] = "vag";
                            break;
                        case 7:
                            rch = c[(c.Length) - 1];
                            if ((rch != ' ') && (rch != '\n') && (rch != '$'))
                                k--;
                            s3 = c.Substring(0, (c.Length) - 1);
                            saved_tokent[m, 0] = s3;
                            saved_tokent[m, 1] = "vaghe   " + k.ToString();
                            saved_tokent[m, 2] = "vag";
                            break;
                        case 8:
                        case 10:
                        case 13:
                            c = c.Substring(0, (c.Length) - 1);
                            k--;
                            saved_tokent[m, 0] = c;
                            saved_tokent[m, 1] = "alaem   " + k.ToString();
                            saved_tokent[m, 2] = c;
                            break;
                        case 14:
                        case 11:
                            rch = c[(c.Length) - 1];
                            if ((rch != ' ') && (rch != '\n') && (rch != '$'))
                                k--;
                            s3 = c.Substring(0, (c.Length) - 1);
                            saved_tokent[m, 0] = s3;
                            saved_tokent[m, 1] = "alaem   " + k.ToString();
                            saved_tokent[m, 2] = s3;
                            break;
                        case 15:
                        case 16:
                            c = c.Substring(0, (c.Length) - 1);
                            k--;
                            m--;
                            tozihat.Items.Add("  "+ c); 
                            break;
                        case 17:
                        case 18:
                        case 20:
                        case 22:
                        case 24:
                        case 27:
                        case 30:
                        case 32:
                                c = c.Substring(0, (c.Length) - 1);
                                k--;
                                saved_tokent[m, 0] = c;
                                saved_tokent[m, 1] = "amalgar " + k.ToString();
                                saved_tokent[m, 2] = c;
                            break;
                        case 21:
                        case 25:
                        case 28:
                        case 31:
                            rch = c[(c.Length)-1];
                              if ((rch != ' ') && (rch != '\n')&&(rch!='$'))
                                  k--;  
                                  s3 = c.Substring(0, (c.Length)-1);
                                  saved_tokent[m, 0] = s3;
                                  saved_tokent[m, 1] = "amalgar " + k.ToString();
                                  saved_tokent[m, 2] = s3;
                            break;
                        case 33:
                        case 34:
                            c = c.Substring(0, (c.Length) - 1);
                            k--;
                            saved_tokent[m, 0] = c;
                            saved_tokent[m, 1] = "reshte  "+k.ToString();
                            saved_tokent[m, 2] = "res";
                            break;
                        default:
                            m--;
                            break;
                    }
                    for (j = 0; j <21; j++)
                        if (reserved[j] == c.Substring(0, (c.Length) - 1))
                        {
                            saved_tokent[m, 0] = c.Substring(0, (c.Length) - 1);
                            saved_tokent[m, 1] ="rezerve"+k.ToString();
                            saved_tokent[m, 2] = c.Substring(0, (c.Length) - 1);
                            break;
                        }
                }
            m++;
            saved_tokent[m, 0] ="akhar";
            saved_tokent[m, 1] = "akhar";
            saved_tokent[m, 2] = "akhar";
            y = m;
            showtoken();
             
        }


    private tokening get_me_token()
    {

        if ((q == 0) || (ok1 == true)) { q = 0; token(); }
        if (q == y) q = 0;
        q++;
        tok.first = saved_tokent[q, 0];
        tok.second = saved_tokent[q, 2];
        ok1 = false;
        return tok;
    }


    private void token_button_Click(object sender, EventArgs e)
    {
        tok = get_me_token();
        textBox1.Text = tok.first + " : " + tok.second;
    }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            tozihat.Items.Clear();
            get_me_token();
            ok1 = true;
        }

    //************************************parser ********************************************
     
     public tokening lookahead,tvz;
     public int[] aq = new int[1000];
     public string[,] nam = new string[20,100];
        public int ac=0,inm1=0,inm2=0,ii=0;
        public bool accept = true;
        

        public void match(string token1)
        {
         
            if (token1==lookahead.second)
                lookahead = get_me_token();
            else
            {
            
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in  " + q.ToString() + ">>  " + lookahead.first + "  jaye: " + token1 + "  *Error match*");
                ac++;///////////////////
                aq[ac] = q-1;////////////////
            }
        }
        //0
        public void S1()
        {
            if (lookahead.second == "barname")
            {
                S(); match("akhar");
                if (accept)
                {
                    pictureBox1.ImageLocation = "accept.png";//@"D:\darsy\term7\compiler_yaghoubi_olom\compiler___yaghoubi\comparser\compilerparser1\compilerscan\bin\Debug\accept.png";
                    pictureBox1.Show();
                    label3.Hide();
                    label2.Show();
                    label4.Hide();
                    listBox4.Hide();
                    //MessageBox.Show("this is accepted in parser.", "ACCEPTE");
                }
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in S1 " + lookahead.first + "  jaye: " + "barname");
                ac++;
                aq[ac] = q - 1;
                
            }
        }
        //1
        public void S()
        {
            if (lookahead.second == "barname")
            {
                nam[inm1, inm2++] = lookahead.first; match("barname"); nam[inm1, inm2++] = lookahead.first; match("ID"); match(";"); Q(inm1);  TA(1); match("shoro"); R(0); match("payan"); match(".");
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in S " + lookahead.first + "  jaye: " + "barname");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //2
        public void Q(int i)
        {
            if ((lookahead.second == "tabe") || (lookahead.second == "shoro")) { }
            else
                if (lookahead.second == "mot")
                {
                    match("mot"); A(i);
                }
                else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in Q " + lookahead.first + "  jaye: " + "{tabe , shoro,mot}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //3
        public void A(int i)
        {
            if ((lookahead.second == "tabe") || (lookahead.second == "shoro")) { }
            else
                if (lookahead.second == "ID" || lookahead.second == "^" || lookahead.second == "array")
                {
                    B(i); match(";"); A(i);
                }else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in A " + lookahead.first + "  jaye: " + "{tabe , shoro,ID, array}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //4
        public void B(int i)
        {
            if (lookahead.second == "ID")
            {
                C(i); match(":"); nam[inm1, inm2++] = lookahead.first; typ(i);
            }
            else if (lookahead.second == "^")
            {
                match("^"); nam[inm1, inm2] = lookahead.first + "  asharegar:>"; match("ID");saved_tokent[q-1,2] = "ID" + i.ToString();tvz.first=saved_tokent[q-1,0];tvz.second=saved_tokent[q-1,2]; taviz(tvz,i);  match(":"); nam[inm1, inm2++] += lookahead.first; typ(i);
            }
            else if (lookahead.second == "array")
            {
                match("array"); nam[inm1, inm2] = lookahead.first + " araye"; match("ID"); saved_tokent[q - 1, 2] = "ID" + i.ToString(); tvz.first = saved_tokent[q - 1, 0]; tvz.second = saved_tokent[q - 1, 2]; taviz(tvz, i); match("["); G(i); match("]"); match(":"); nam[inm1, inm2++] += lookahead.first; typ(i);
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in B: " + lookahead.first + "  jaye: " + "{ID,^,array}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //5
        public void C(int i)
        {
            if (lookahead.second == "ID")
            {
               nam[inm1,inm2++] = lookahead.first;
               match("ID"); saved_tokent[q - 1, 2] = "ID" + i.ToString(); tvz.first = saved_tokent[q - 1, 0]; tvz.second = saved_tokent[q - 1, 2]; taviz(tvz, i); CC(i);
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in C: " + lookahead.first + "  jaye: " + "{ID}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //6
        public void CC(int i)
        {
            if (lookahead.second == ":") { }
            else
                if (lookahead.second == ",")
                {
                    match(","); C(i);
                }
            else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in CC: " + lookahead.first + "  jaye: " + "{,,:}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //7
        public void G(int i)
        {
            if (lookahead.second == "sah")
            {
                nam[inm1, inm2] += lookahead.first; match("sah"); match(".."); nam[inm1, inm2] +=".."+ lookahead.first+","; match("sah"); GG(i);
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in G: " + lookahead.first + "  jaye: " + "{sah}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //8
        public void GG(int i)
        {
            if (lookahead.second == "]")
            { }
            else
                if(lookahead.second==",")
            {
                match(","); inm2++; G(i);
            }
                else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in GG: " + lookahead.first + "  jaye: " + "{],,}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //9
        public void TA(int i)
        {
            inm2 = 0;
            if (lookahead.second == "shoro")
            {}
            else
                if (lookahead.second == "tabe")
                {
                    inm1++; match("tabe"); nam[inm1, inm2++] = lookahead.first + " tabe:"; match("ID"); saved_tokent[q - 1, 2] = "ID0"; tvz.first = saved_tokent[q - 1, 0]; tvz.second = saved_tokent[q - 1, 2]; taviz(tvz,0); match("("); PA(i); match(")"); match(":"); nam[inm1, 0] += lookahead.first; typ(i); match(";"); Q(i); match("shoro"); R(i); match("payan"); match(";"); TA(++i);
                }
                else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in TA: " + lookahead.first + "  jaye: " + "{shoro,tabe}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //10
        public void PA(int i)
        {
            if (lookahead.second == ")")
            {}
            else if (lookahead.second == "mot")
            {
                match("mot"); C(i); match(":"); nam[inm1, inm2++] = lookahead.first; typ(i); match(";"); PA(i);
            }
            else
                if (lookahead.second == "ID")
                {
                    C(i); match(":"); nam[inm1, inm2++] = lookahead.first; typ(i); match(";"); PA(i);
                }
                else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in PA: " + lookahead.first + "  jaye: " + "{ID,),mot}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //11
        public void R(int i)
        {
            if (lookahead.second == "payan")
            { }
            else
                if (lookahead.second == "^" || lookahead.second == "ID"+i.ToString() || lookahead.second == "agar" || lookahead.second == "tavaghty" || lookahead.second == "baraye" || lookahead.second == "@" || lookahead.second == "benvis" || lookahead.second == "bekhan")
                {
                    P(i); R(i);
                }
                else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in R: " + lookahead.first + "  jaye: " + "{ID,^,payan,@,bekhan,tavaghty,agar,baraye}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //12
        public void P(int i)
        {
            if (lookahead.second == "bekhan" || lookahead.second == "benvis") N(i);
            else
                if (lookahead.second == "baraye") FO(i);
                else
                    if (lookahead.second == "tavaghty") V(i);
                    else
                        if (lookahead.second == "agar") IU(i);
                        else
                            if (lookahead.second == "^" || lookahead.second == "ID"+i.ToString() || lookahead.second == "@")
                                ST(i);
                            else
                            {
                                accept = false;
                                listBox4.Items.Add(ac.ToString() + ">Error in P: " + lookahead.first + "  jaye: " + "{ID,^,@,bekhan,tavaghty,agar,baraye}");
                                ac++;
                                aq[ac] = q - 1;
                            }
        }
        //13
        public void ST(int i)
        {
            if (lookahead.second == "^" || lookahead.second == "ID" + i.ToString() || lookahead.second == "@")
            { LS(i); match(":="); RS(i); match(";"); }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in ST: " + lookahead.first + "  jaye: " + "{ID,^,@}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //14
        public void LS(int i)
        {
            if(lookahead.second=="^")
            {
                match("^"); match("ID" + i.ToString());
            }
            else if (lookahead.second == "ID" + i.ToString())
            {
                match("ID" + i.ToString()); LF(i); 
            }
            else if(lookahead.second=="@")
            { match("@"); match("ID" + i.ToString()); }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in LS: " + lookahead.first + "  jaye: " + "{ID,^,@}");
                ac++;
                aq[ac] = q - 1;
            }
        }
     //15
        public void LF(int i)
        {
            if (lookahead.second == ":=")
            { }
            else if (lookahead.second == "[")
            { match("["); AA(i); match("]"); }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in LF: " + lookahead.first + "  jaye: " + "{[,:=}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //16
        public void AA(int i)
        {
            if (lookahead.second == "ID" + i.ToString() || lookahead.second == "sah")
            {
                match(lookahead.second); A1(i);
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in AA: " + lookahead.first + "  jaye: " + "{ID,sah}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //17
        public void A1(int i)
        {
            if (lookahead.second == "]")
            { }
            else
                if (lookahead.second == ",")
                { match(","); AA(i); }
                else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in A1: " + lookahead.first + "  jaye: " + "{],,}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //18
        public void RS(int i)
        {
            if (lookahead.second == "res" || lookahead.second == "tohy")
            { match(lookahead.second); }
            else if (lookahead.second == "@" || lookahead.second == "^")
            {
                match(lookahead.second); match("ID" + i.ToString());
            }
            else if (lookahead.second == "boly")
            { match("boly"); W(i); }
            else if (lookahead.second == "ID" + i.ToString() || lookahead.second == "sah" || lookahead.second == "vag" || lookahead.second == "(" || lookahead.second == "-" || lookahead.second == "+")
            { D(i); }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in RS: " + lookahead.first + "  jaye: " + "{ID,^,@,res,(,vag,sah,boly,+,-}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //19
        public void D(int i)
        {
            if (lookahead.second == "ID" + i.ToString() || lookahead.second == "sah" || lookahead.second == "vag" || lookahead.second == "(" || lookahead.second == "-" || lookahead.second == "+")
            {
                DT(i); TS(i);
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in D: " + lookahead.first + "  jaye: " + "{ID,sah,vag,(,+,-}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //20
        public void TS(int i)
        {
            if (lookahead.second == ";" || lookahead.second == ")")
            { }
            else
                if (lookahead.second == "+" || lookahead.second == "-")
                {
                    match(lookahead.second); D(i);
                }
                else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in TS: " + lookahead.first + "  jaye: " + "{ID,sah,vag,(,+,-}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //21
        public void DT(int i)
        {
            if (lookahead.second == "ID" + i.ToString() || lookahead.second == "sah" || lookahead.second == "vag" || lookahead.second == "(" || lookahead.second == "-" || lookahead.second == "+")
            {
                E(i); F(i);
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in DT: " + lookahead.first + "  jaye: " + "{ID,sah,vag,(,+,-}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //22
        public void F(int i)
        {
            if (lookahead.second == ";" || lookahead.second == ")" || lookahead.second == "-" || lookahead.second == "+")
            { }
            else
                if (lookahead.second == "/" || lookahead.second == "\\" || lookahead.second == "*" || lookahead.second == "%")
                {
                    OA(i); DT(i);
                }
                else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in F: " + lookahead.first + "  jaye: " + "{+,-,*,/,\\,%,;,)}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //23
        public void OA(int i)
        {
            if (lookahead.second == "/" || lookahead.second == "\\" || lookahead.second == "*" || lookahead.second == "%")
                match(lookahead.second);
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in OA: " +q.ToString()+" >> "+ lookahead.first + "  jaye: " + "{+,-,*,/,\\,%,;,)}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //24
        public void E(int i)
        {
            if (lookahead.second == "ID" + i.ToString() || lookahead.second == "sah" || lookahead.second == "vag")
            {
                match(lookahead.second);
            }
            else if (lookahead.second == "(")
            {
                match("("); D(i); match(")");
            }
            else if (lookahead.second == "+" || lookahead.second=="-")
            {
                match(lookahead.second); L(i);
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in E: " + lookahead.first + "  jaye: " + "{ID,sah,vag,(,+,-}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //25
        public void IU(int i)
        {
            if (lookahead.second == "agar")
            {
                match("agar"); W(i); match("angah"); match("shoro"); R(i); match("payan"); IV(i);
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in IU: " + lookahead.first + "  jaye: " + "{agar}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //26
        public void IV(int i)
        {
            if (lookahead.second == "vagarna")
            {
                match("vagarna"); match("shoro"); R(i); match("payan"); match(";");
            }
            else if (lookahead.second == ";")
                match(";");
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in IV: " + lookahead.first + "  jaye: " + "{;,vagarna}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //27
        public void N(int i)
        {
            if (lookahead.second == "benvis")
            {
                match("benvis"); match("("); RS(i); match(")"); match(";");
            }
            else if (lookahead.second == "bekhan")
            {
                match("bekhan"); match("("); LS(i); match(")"); match(";");
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in N: " + lookahead.first + "  jaye: " + "{bekhan,benvis}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //28
        public void V(int i)
        {
            if (lookahead.second == "tavaghty")
            {
                match("tavaghty"); W(i); match("anjam"); match("shoro"); R(i); match("payan"); match(";");
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in V: " + lookahead.first + "  jaye: " + "{tavaghty}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //29
        public void FO(int i)
        {
            if(lookahead.second=="baraye")
            {
                match("baraye"); match("("); match("ID" + i.ToString()); match(":="); match("sah"); match(";"); match("sah"); match(")"); match("anjam"); match("shoro"); R(i); match("payan"); match(";");
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in FO: " + lookahead.first + "  jaye: " + "{baraye}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //30
        public void W(int i)
        {
            if (lookahead.second == "ID" + i.ToString())
                match("ID" + i.ToString());
            else
                if(lookahead.second=="(")
                {
                    match("("); HH(i); match(")");
                }
            else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in W: " + lookahead.first + "  jaye: " + "{ID,(}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //31
        public void HH(int i)
        {
            if (lookahead.second == "dorost" || lookahead.second == "ghalat")
                match(lookahead.second);
            else if (lookahead.second == "!")
            {
                match("!"); match("("); HH(i); match(")");
            }
            else if (lookahead.second == "res" || lookahead.second == "(" || lookahead.second == "ID" + i.ToString() || lookahead.second == "sah" || lookahead.second == "vag" || lookahead.second == "+" || lookahead.second == "-")
                H(i);
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in HH: " + lookahead.first + "  jaye: " + "{ID,dorost,ghalat,!,sah,vag,res,+,-,(}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //32
        public void H(int i)
        {
            if (lookahead.second == "(")
            {
                match("("); HH(i); match(")"); OM(i); match("("); HH(i); match(")");
            }
            else
                if (lookahead.second == "res" || lookahead.second == "ID" + i.ToString() || lookahead.second == "sah" || lookahead.second == "vag" || lookahead.second == "+" || lookahead.second == "-")
                    I(i);
                else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in H: " + lookahead.first + "  jaye: " + "{ID,sah,vag,res,+,-,(}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //33
        public void OM(int i)
        {
            if (lookahead.second == "||" || lookahead.second == "&&")
                match(lookahead.second);
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in OM: " + lookahead.first + "  jaye: " + "{||,&&}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //34
        public void I(int i)
        {
            if (lookahead.second == "res" || lookahead.second == "ID" + i.ToString() || lookahead.second == "sah" || lookahead.second == "vag" || lookahead.second == "+" || lookahead.second == "-")
            {
                J(i); K(i); J(i);
            }
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in I: " + lookahead.first + "  jaye: " + "{ID,sah,vag,res,+,-}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //35
        public void J(int i)
        {
            if (lookahead.second == "res" || lookahead.second == "ID" + i.ToString() || lookahead.second == "sah" || lookahead.second == "vag")
                match(lookahead.second);
            else
                if(lookahead.second=="+" || lookahead.second=="-")
                {
                    match(lookahead.second); L(i);
                }
            else
                {
                    accept = false;
                    listBox4.Items.Add(ac.ToString() + ">Error in J: " + lookahead.first + "  jaye: " + "{ID,sah,vag,res,+,-}");
                    ac++;
                    aq[ac] = q - 1;
                }
        }
        //36
        public void L(int i)
        {
            if (lookahead.second == "ID" + i.ToString() || lookahead.second == "sah" || lookahead.second == "vag")
                match(lookahead.second);
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in L: " + lookahead.first + "  jaye: " + "{ID,sah,vag}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //37
        public void K(int i)
        {
            if(lookahead.second==">" ||lookahead.second == ">=" || lookahead.second == "<" || lookahead.second == "<="||lookahead.second == "=" || lookahead.second == "<>")
                match(lookahead.second);
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in K: " + lookahead.first + "  jaye: " + "{<,>,<=,>=,=,<>}");
                ac++;
                aq[ac] = q - 1;
            }
        }
        //38
        public void typ(int i)
        {
            if (lookahead.second == "sahih" || lookahead.second == "reshte" || lookahead.second == "boly" || lookahead.second == "vaghe")
                match(lookahead.second);
            else
            {
                accept = false;
                listBox4.Items.Add(ac.ToString() + ">Error in typ: " + lookahead.first + "  jaye: " + "{sahih,vaghe,reshte,boly}");
                ac++;
                aq[ac] = q - 1;
            }
        }

        public void taviz(tokening sta, int j)
        {
            for (int i = 0; i <= y; i++)
                if (saved_tokent[i, 0] != null)
                {
                    if (saved_tokent[i, 0] == sta.first)
                        saved_tokent[i, 2] = "ID" + j.ToString();
                }
        }

        private void button1_Click(object sender, EventArgs e)//parse_button
        {
            inm1 = 0;
            inm2 = 0;
            Array.Clear(nam, 0, 2000);
            accept = true;
            tozihat.Items.Clear();
            listBox4.Items.Clear();
            listBox4.ForeColor = Color.Black;
            ac = 0;
            if (q != 0) q = 0; 
              lookahead = get_me_token();
             S1();
             if (!accept)
             {
                 pictureBox1.ImageLocation = "not_accept.png";//@"D:\darsy\term7\compiler_yaghoubi_olom\compiler___yaghoubi\comparser\compilerparser1\compilerscan\bin\Debug\not_accept.png";
                 pictureBox1.Show();
                 label2.Hide();
                 label3.Show();
                 listBox4.Show();
                 label4.Show();
             }
             MessageBox.Show("Finished.");
             button3.Select();
             okx = false;
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex=aq[listBox4.SelectedIndex+1];
            listBox2.SelectedIndex = aq[listBox4.SelectedIndex+1];
            listBox4.ForeColor = Color.DarkMagenta;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            listBox2.SelectedIndex= listBox1.SelectedIndex;
           // richTextBox1.SelectedText =  listBox1.SelectedItem.ToString().Substring(2,listBox1.Text.Length-2);
        }
        public  int xx = 0;
        public bool okx = false;
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int ll = listBox2.SelectedIndex.ToString().Length + 10;
                string s12 = listBox2.SelectedItem.ToString();
                s12 = s12.Substring(ll, s12.Length - ll);
                listBox1.SelectedIndex = listBox2.SelectedIndex;
                xx = int.Parse(s12);
                okx = true;
                richTextBox1.Select(xx - 3, 3);
            }
            catch{}
        }

        private void richTextBox1_MouseLeave(object sender, EventArgs e)
        {
            if (okx == true)
            {
                richTextBox1.Select(xx - 3, 3);
            }
        }

        private void listBox4_MouseLeave(object sender, EventArgs e)
        {
            listBox4.ForeColor = Color.Black;
        }

        private void Form1_Enter(object sender, EventArgs e)
        {
            button1.Select();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = @"barname  mn; 
            mot a1:reshte;
            array ar[1 ..3]:sahih;
            ^p:vaghe;
            ok:boly;
            b,t,a:sahih;d:vaghe;
            tabe dd(s,a3:sahih;mot a11,z:boly;):sahih;
            mot x:vaghe;
            shoro x:=12+76-(54*3);
            payan;
            shoro
            agar(12>=a)angah shoro a:=1;
            payan vagarna
            shoro payan;
            a:=@b;
            d:=12.e2+12.1*-1+-2%6+(12-1);
            ^p:=boly((!(ok >1))&&(!(1<>t)));
            t:=a%t*(2-8)/98*2;
            tavaghty ok anjam
            shoro
            p:=@b;
            d:=12.e2+12.1*-1+-2%6+(12-1);
            agar(12>=a)angah shoro 
            agar(12>=a)angah shoro p:=@b; payan;
            a:=1;
            payan vagarna
                shoro
               baraye(t:=0;10)anjam shoro 
                d:=(-1+ 23)*2;              payan; 

                bekhan(ar[1,1]);
                benvis(d+12-1+(7+2));
                payan;
            payan;
            payan.

            ";
        }

        private void richTextBox1_MouseEnter(object sender, EventArgs e)
        {
            if (okx == true)
            {
                richTextBox1.Select(xx - 3, 3);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
         //   if (accept)
         //   {
            inm2 = 0; int i = 0;
                tozihat.Items.Clear();
                for (inm1 = 0; inm1 < 20; inm1++)
                {
                    inm2 = 0;
                    while (nam[inm1, inm2] != null && inm2 != 100)
                    {
                        tozihat.Items.Add(inm1.ToString()+":  "+ nam[inm1, inm2]);
                        inm2++;
                    }
                }
           // }
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            if (okx == true)
            {
              if(xx!=0)  richTextBox1.Select(xx - 3,3); 
            }
        }

        private void richTextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            richTextBox1.DeselectAll();
            okx = false;
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            richTextBox1.Select();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Email:mohammad_khanjani2007@yahoo.comمحمد خانجانی و عباس قاسمی ـ  ", "کدنویسان", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        
    }
}
    
        