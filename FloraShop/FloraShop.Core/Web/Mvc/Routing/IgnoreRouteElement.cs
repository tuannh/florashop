#region License
// 
// Copyright (c) 2013, FloraShop.Core team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
/*
FloraShop.Core is a content management system based on ASP.NET MVC framework. Copyright 2009 Yardi Technology Limited.

This program is free software: you can redistribute it and/or modify it under the terms of the
GNU General Public License version 3 as published by the Free Software Foundation.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.
If not, see http://www.FloraShop.Core.com/gpl3/.
*/
using System;
using System.Collections;
using System.Configuration;
namespace FloraShop.Core.Web.Mvc.Routing
{
    public class IgnoreRouteElement : ConfigurationElement
    {
        public IgnoreRouteElement() { }
        public IgnoreRouteElement(string elementName)
        {
            Name = elementName;
        }
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }

        [ConfigurationProperty("constraints", IsRequired = false)]
        public RouteChildElement Constraints
        {
            get { return (RouteChildElement)this["constraints"]; }
            set { this["constraints"] = value; }
        }
    }
}

