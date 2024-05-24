using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PXWeb.Admin
{
    public partial class Settings_General_Administration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));

            if (!IsPostBack)
            {
                ReadSettings();
            }

            lblAddNewIPAddressError.Text = "";
        }

        /// <summary>
        /// Read and display Administration settings  
        /// </summary>
        private void ReadSettings()
        {
            cboUseIPFilter.SelectedValue = PXWeb.Settings.Current.General.Administration.UseIPFilter.ToString();
            rptIPAddresses.DataSource = PXWeb.Settings.Current.General.Administration.IPAddresses;
            rptIPAddresses.DataBind();
        }

        /// <summary>
        /// Save Administration settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MasterSave_Click(Object sender, System.EventArgs e)
        {
            TextBox txt;
            Label lbl;
            if (Page.IsValid)
            {
                if (PXWeb.Settings.BeginUpdate())
                {
                    try
                    {
                        PXWeb.AdministrationSettings adm = (PXWeb.AdministrationSettings)PXWeb.Settings.NewSettings.General.Administration;

                        adm.UseIPFilter = bool.Parse(cboUseIPFilter.SelectedValue);

                        List<string> ipAddresses = (List<string>)adm.IPAddresses;
                        ipAddresses.Clear();

                        foreach (RepeaterItem ip in rptIPAddresses.Items)
                        {
                            txt = (TextBox)ip.FindControl("txtIPAddress");
                            lbl = (Label)ip.FindControl("lblError");
                            if (txt != null)
                            {
                                if (txt.Text.Length > 0)
                                {
                                    if (VerifyIPAddress(txt.Text))
                                    {
                                        ipAddresses.Add(txt.Text);
                                        if (lbl != null)
                                        {
                                            lbl.Text = "";
                                        }
                                    }
                                    else
                                    {
                                        if (lbl != null)
                                        {
                                            lbl.Text = Master.GetLocalizedString("PxWebAdminSettingsAdministrationInvalidIPAddress");
                                        }
                                        return;
                                    }
                                }
                            }
                        }


                        PXWeb.Settings.Save();
                        ReadSettings();
                    }
                    finally
                    {
                        PXWeb.Settings.EndUpdate();
                    }
                }
            }
        }

        /// <summary>
        /// Add a new IP Address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddNewIPAddress(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();
            TextBox txt;
            //Label lbl;

            foreach (RepeaterItem ip in rptIPAddresses.Items)
            {
                txt = (TextBox)ip.FindControl("txtIPAddress");
                if (txt != null)
                {
                    //if (txt.Text.Length > 0)
                    //{
                    //    if (VerifyIPAddress(txt.Text))
                    //    {
                    lst.Add(txt.Text);
                    //}
                    //else
                    //{
                    //    lbl = (Label)ip.FindControl("lblError");
                    //    lbl.Text = Master.GetLocalizedString("PxWebAdminSettingsAdministrationInvalidIPAddress");
                    //    return;
                    //}
                    //}
                }
            }

            if (VerifyIPAddress(txtAddNewIPAddress.Text))
            {
                lst.Add(txtAddNewIPAddress.Text);
            }
            else
            {
                lblAddNewIPAddressError.Text = Master.GetLocalizedString("PxWebAdminSettingsAdministrationInvalidIPAddress");
                return;
            }

            rptIPAddresses.DataSource = null;
            rptIPAddresses.DataSource = lst;
            rptIPAddresses.DataBind();

            txtAddNewIPAddress.Text = "";
        }


        /// <summary>
        /// Verifies that a IP address has the right format
        /// </summary>
        /// <param name="ip">The ip address string to verify</param>
        /// <returns>True if ip is a valid IP address, else false</returns>
        private bool VerifyIPAddress(string ip)
        {
            char[] chars = ip.ToCharArray();
            int dots = 0;
            string[] numbers;
            char[] delimiters = new char[] { '.' };

            // Only digits and "." allowed
            for (int i = 0; i < ip.Length; i++)
            {
                if (!char.IsDigit(ip, i))
                {
                    if (chars[i].Equals('.'))
                    {
                        dots = dots + 1;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            // Only 3 dots allowed
            if (dots > 3)
            {
                return false;
            }

            // Numbers cannot be bigger than 255
            numbers = ip.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < numbers.Length; i++)
            {
                if (int.Parse(numbers[i]) > 255)
                {
                    return false;
                }
            }

            return true;
        }

        protected void UseIPFilterInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsAdministrationUseIPFilter", "PxWebAdminSettingsAdministrationUseIPFilterInfo");
        }
        protected void IPAddressesInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsAdministrationIPAddresses", "PxWebAdminSettingsAdministrationIPAddressesInfo");
        }

    }
}
