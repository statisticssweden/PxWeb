using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using PX.Security;
using log4net;

namespace PXWeb
{
    public class ProtectionSettings : IProtection
    {

        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProtectionSettings));

        private bool _protected = false;
        private PX.Security.IAuthorization _authorizer;
        private string _authorizationMethod;

        public bool IsProtected
        {
            get { return _protected; }
            set { _protected = value; }
        }

        PX.Security.IAuthorization IProtection.AuthorizationMethod
        {
            get { return _authorizer; }
        }

        public void SetAuthorizationMethod(string method)
        {
            _authorizationMethod = method;

            if (string.IsNullOrEmpty(_authorizationMethod))
            {
                _authorizer = new PXWeb.Security.DefaultAuthorization();
                _logger.Info("Using DefaultAuthorization");
            }
            else
            {
                var typeString = _authorizationMethod;
                var parts = typeString.Split(',');
                var typeName = parts[0].Trim();
                var assemblyName = parts[1].Trim();
                _authorizer = (IAuthorization)Activator.CreateInstance(assemblyName, typeName).Unwrap();
                _logger.Info("Using " + typeString);
            }
        }

        public string GetAuthorizationMethod()
        {
            return _authorizationMethod;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="menuNode">XML-node for the Search index settings</param>
        public ProtectionSettings(XmlNode indexNode)
        {
            string xpath;

            xpath = "./isProtected";
            IsProtected = SettingsHelper.GetSettingValue(xpath, indexNode, false);

            xpath = "./authorizationMethod";
            SetAuthorizationMethod(SettingsHelper.GetSettingValue(xpath, indexNode, ""));

        }

        /// <summary>
        /// Save the Search index settings to the settings file
        /// </summary>
        /// <param name="menuNode">XML-node for the search index settings</param>
        public void Save(XmlNode indexNode)
        {
            string xpath;

            xpath = "./isProtected";
            SettingsHelper.SetSettingValue(xpath, indexNode, IsProtected.ToString());

            xpath = "./authorizationMethod";
            SettingsHelper.SetSettingValue(xpath, indexNode, GetAuthorizationMethod());
        }
    }
}