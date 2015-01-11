﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using System.Configuration;
using FloraShop.Core.Utility;
using FloraShop.Core.Configurations;

namespace FloraShop.Core.Web.Mvc.Routing
{
    public class RouteTableRegister
    {
        private sealed class IgnoreRouteInternal : Route
        {
            // Methods
            public IgnoreRouteInternal(string url)
                : base(url, new StopRoutingHandler())
            {
            }

            public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary routeValues)
            {
                return null;
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            string config = SiteConfiguration.GetConfig().ConfigFile.Route;
            config = Globals.MapPath(config);

            RegisterRoutes(routes, config);
        }
        public static void RegisterRoutes(RouteCollection routes, string routeFile)
        {
            lock (routes)
            {
                RouteTableSection routesTableSection = GetRouteTableSection(routeFile);
                if (routesTableSection != null)
                {
                    //ignore route
                    if (routesTableSection.Ignores.Count > 0)
                    {
                        foreach (ConfigurationElement item in routesTableSection.Ignores)
                        {
                            var ignore = new IgnoreRouteInternal(((IgnoreRouteElement)item).Url)
                            {
                                Constraints = GetRouteValueDictionary(((IgnoreRouteElement)item).Constraints.Attributes)
                            };
                            routes.Add(ignore);
                        }
                    }

                    if (routesTableSection.Routes.Count > 0)
                    {
                        for (int routeIndex = 0; routeIndex < routesTableSection.Routes.Count; routeIndex++)
                        {
                            RouteConfigElement route = routesTableSection.Routes[routeIndex] as RouteConfigElement;
                            if (routes[route.Name] == null)
                            {
                                if (string.IsNullOrEmpty(route.RouteType))
                                    routes.Add(route.Name, !string.IsNullOrEmpty(route.RedirectUrl) ? 
                                            new Route(route.Url, 
                                                new RedirectRouteHandler(route.RedirectUrl)) :
                                            new Route(
                                                        route.Url,
                                                        GetDefaults(route),
                                                        GetConstraints(route),
                                                        GetDataTokens(route),
                                                        GetInstanceOfRouteHandler(route)));
                                
                                else
                                {
                                    var customRoute = (RouteBase)Activator.CreateInstance(Type.GetType(route.RouteType),
                                                        route.Url,
                                                        GetDefaults(route),
                                                        GetConstraints(route),
                                                        GetDataTokens(route),
                                                        GetInstanceOfRouteHandler(route));
                                    routes.Add(route.Name, customRoute);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static RouteTableSection GetRouteTableSection(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return (RouteTableSection)ConfigurationManager.GetSection("routeTable");
            }
            else
            {
                var section = (RouteTableSection)Activator.CreateInstance(typeof(RouteTableSection));

                section.DeserializeSection(IOUtility.ReadAsString(file));

                return (RouteTableSection)section;
            }
        }

        /// <summary>
        /// Gets the instance of route handler.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private static IRouteHandler GetInstanceOfRouteHandler(RouteConfigElement route)
        {
            IRouteHandler routeHandler;

            if (string.IsNullOrEmpty(route.RouteHandlerType))
                routeHandler = new MvcRouteHandler();
            else
            {
                try
                {
                    Type routeHandlerType = Type.GetType(route.RouteHandlerType);
                    routeHandler = Activator.CreateInstance(routeHandlerType) as IRouteHandler;
                }
                catch (Exception e)
                {
                    throw new ApplicationException(
                                 string.Format("Can't create an instance of IRouteHandler {0}", route.RouteHandlerType),
                                 e);
                }

            }

            return routeHandler;
        }


        /// <summary>
        /// Gets the constraints.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private static RouteValueDictionary GetConstraints(RouteConfigElement route)
        {
            return GetRouteValueDictionary(route.Constraints.Attributes);
        }


        /// <summary>
        /// Gets the defaults.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private static RouteValueDictionary GetDefaults(RouteConfigElement route)
        {
            return GetRouteValueDictionary(route.Defaults.Attributes);
        }

        /// <summary>
        /// Gets the data tokens.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private static RouteValueDictionary GetDataTokens(RouteConfigElement route)
        {
            return GetRouteValueDictionary(route.DataTokens.Attributes);
        }

        /// <summary>
        /// Gets the dictionary from attributes.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        private static RouteValueDictionary GetRouteValueDictionary(Dictionary<string, string> attributes)
        {
            RouteValueDictionary dataTokensDictionary = new RouteValueDictionary();

            foreach (var dataTokens in attributes)
            {
                //ref : DefaultControllerFactory.GetControllerType
                if (dataTokens.Value == "UrlParameter.Optional")
                {
                    dataTokensDictionary.Add(dataTokens.Key, UrlParameter.Optional);
                }
                else if (dataTokens.Key == "Namespaces")
                {
                    dataTokensDictionary.Add(dataTokens.Key, dataTokens.Value.Split(','));
                }
                else
                {
                    dataTokensDictionary.Add(dataTokens.Key, dataTokens.Value);
                }
            }

            return dataTokensDictionary;

        }
    }
}

