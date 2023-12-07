using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SimpleWordTest_comb_t : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["count"] = 0;
            Session["quizcount"] = 0;
            Session["correct"] = 0;
            Session["allcount"] = 0;
        }
    }
    int count, quizcount, correct, allcount = 0;
    



    protected void CBF110004_DDL1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CBF110004_cambridge.Text += $"<a href = 'https://dictionary.cambridge.org/zht/%E8%A9%9E%E5%85%B8/%E8%8B%B1%E8%AA%9E-%E6%BC%A2%E8%AA%9E-%E7%B9%81%E9%AB%94/{CBF110004_DDL1.SelectedItem}' target='_blank'>{CBF110004_DDL1.SelectedItem}</a>  ==＞ {CBF110004_DDL1.SelectedValue}" + "<br />";
    }

    protected void CBF110004_DDL1_DataBound(object sender, EventArgs e)
    {
        CBF110004_cambridge.Text += $"<a href = 'https://dictionary.cambridge.org/zht/%E8%A9%9E%E5%85%B8/%E8%8B%B1%E8%AA%9E-%E6%BC%A2%E8%AA%9E-%E7%B9%81%E9%AB%94/{CBF110004_DDL1.SelectedItem}' target='_blank'>{CBF110004_DDL1.SelectedItem}</a>  ==＞ {CBF110004_DDL1.SelectedValue}" + "<br />";
        if (!IsPostBack)
        {
            for (int i = 0; i < CBF110004_GV1.PageCount; i++)
            {
                allcount = (int)Session["allcount"];
                allcount += CBF110004_GV1.Rows.Count;
                Session["allcount"] = allcount;
                CBF110004_GV1.PageIndex++;
            }
            /*
            Response.Write(allcount+"<br />");//檢查allcount用
            Response.Write(count + "<br />");//檢查count用
            */
            CBF110004_GV1.PageIndex = 0;
        }

    }

    protected void CBF110004_PreviousButton_Click(object sender, EventArgs e)
    {
        CBF110004_cambridge.Text = "";
        count = (int)Session["count"];
        count -= 10;
        Session["count"] = count;
        SqlDataSource1.SelectCommand = $"SELECT * FROM [gept_words] ORDER BY [id] OFFSET {count} ROWS FETCH NEXT 10 ROWS ONLY";
        CBF110004_DDL1.DataBind();
        if (count == 0)
        {
            CBF110004_PreviousButton.Enabled = false;
        }
        if (CBF110004_DDL1.Items.Count == 10)
        {
            CBF110004_NextButton.Enabled = true;
        }
    }

    protected void CBF110004_NextButton_Click(object sender, EventArgs e)
    {
        CBF110004_cambridge.Text = "";
        count = (int)Session["count"];
        count += 10;
        Session["count"] = count;

        allcount = (int)Session["allcount"];
        Session["allcount"] = allcount;

        SqlDataSource1.SelectCommand = $"SELECT * FROM [gept_words] ORDER BY [id] OFFSET {count} ROWS FETCH NEXT 10 ROWS ONLY";
        CBF110004_DDL1.DataBind();
        if (count != 0)
        {
            CBF110004_PreviousButton.Enabled = true;
        }
        
        if ((count + CBF110004_DDL1.Items.Count) == allcount)//問題點(已解決)
        {
            CBF110004_NextButton.Enabled = false; 
        }
        if (CBF110004_DDL1.Items.Count != 10)
        {
            CBF110004_NextButton.Enabled = false;
        }
        allcount = (int)Session["allcount"];
        Session["allcount"] = allcount;

        /*
        Response.Write(allcount+"<br />");//檢查allcount用
        Response.Write(count + "<br />");//檢查count用
        */
    }

    protected void CBF110004_GV1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        count = (int)Session["count"];
        Session["count"] = count;
        SqlDataSource1.SelectCommand = $"SELECT * FROM [gept_words] ORDER BY [id] OFFSET {count} ROWS FETCH NEXT 10 ROWS ONLY";
        CBF110004_cambridge.Text = "";
        CBF110004_DDL1.DataBind();

    }

    static string[] quizs = new string[10];

    void Shuffle()
    {
        for (int i = 0; i < CBF110004_DDL1.Items.Count; i++)
        {
            quizs[i] = CBF110004_DDL1.Items[i].ToString();
            //CBF110004_ch_hint.Text += $"第{i}項為 {quizs[i]} <br />"; //檢查用
        }
        //CBF110004_ch_hint.Text += "<br />";
        Random r = new Random();
        for (int i = quizs.Length - 1; i >= 1; i--)
        {
            int x = r.Next(i + 1);
            if (x != i)
            {
                string tmp = quizs[x];
                quizs[x] = quizs[i];
                quizs[i] = tmp;
            }
        }
        /*
        for (int i = 0; i < quizs.Length; i++)
        {
            CBF110004_ch_hint.Text += $"第{i}項為 {quizs[i]} <br />"; //檢查用
        }
        */
    }

    void Check()
    {
        correct = (int)Session["correct"];
        quizcount = (int)Session["quizcount"];
        if (quizcount >= 10)
        {

        }
        else
        {
            if (CBF110004_input.Text == quizs[quizcount])
            {
                correct++;
                CBF110004_ch_hint.Text = "答對了！ <br />";
            }
            else
            {
                if (quizcount == 9)
                {
                    CBF110004_ch_hint.Text = $"答錯了！答案是 {quizs[quizcount]} ";
                }
                else
                {
                    CBF110004_ch_hint.Text = $"答錯了！答案是 {quizs[quizcount]} <br />";
                }
                
            }
        }

        quizcount++;
        Session["quizcount"] = quizcount;
        Session["correct"] = correct;
    }

    void Showquiz()
    {
        quizcount = (int)Session["quizcount"];
        Session["quizcount"] = quizcount;
        CBF110004_input.Text = "";

        for (int i = 0; i < quizs[quizcount].Length; i++)
        {
            if (i == 0)
            {
                CBF110004_input.Text += quizs[quizcount][i];
            }
            else
            {
                CBF110004_input.Text += "＿";
            }
        }
        for (int j = 0; j < CBF110004_DDL1.Items.Count ; j++)
        {
            if (quizs[quizcount] == CBF110004_DDL1.Items[j].Text)
            {
                CBF110004_ch_hint.Text += CBF110004_DDL1.Items[j].Value;
            }
        }
    }
    protected void CBF110004_testBtn_Click(object sender, EventArgs e)
    {
        CBF110004_MV1.ActiveViewIndex = 1;
        Shuffle();
        Showquiz();
        //CBF110004_ch_hint.Text += "<br /> ";

        /*
        for (int i = 0; i < quizs.Length; i++)
        {
            CBF110004_ch_hint.Text += $"第{i}項為 {quizs[i]} <br />"; //檢查用
        }
        */
    }
    protected void CBF110004_nextQBtn_Click(object sender, EventArgs e)
    {
        if(quizcount == 9)
        {
            //quizcount -= 1;
        }
        else
        {
            Check();
        }
        if (quizcount < 10)
        {
            Showquiz();
        }
        else
        {
            CBF110004_ch_hint.Text += $"總得分：{correct*10:f}";
            CBF110004_input.Visible = false;
            CBF110004_nextQBtn.Text = "結束";
            reset.Style.Clear();
        }

        if(quizcount > 10)
        {
            //CBF110004_MV1.Visible = false;
            Response.End();
        }

    }

}
