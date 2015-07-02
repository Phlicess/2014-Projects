using System;
using System.Web.Mvc;

namespace FMS.Common
{
    /// <summary>
    /// Executes a specific action result depending on whether the incoming request is an Ajax request or not
    /// </summary>
    public class AjaxableActionResult : ActionResult
    {
        /// <summary>
        /// The result to execute for non-Ajax requests
        /// </summary>
        public Func<ActionResult> DefaultResult { get; set; }

        /// <summary>
        /// The result the execute for Ajax requests
        /// </summary>
        public Func<ActionResult> AjaxResult { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (DefaultResult == null)
                throw new ArgumentException("The DefaultResult property must be set");

            if (AjaxResult == null)
                throw new ArgumentException("The AjaxResult property must be set");


            if (context.HttpContext.Request.IsAjaxRequest())
            {
                AjaxResult().ExecuteResult(context);
            }
            else
            {
                DefaultResult().ExecuteResult(context);
            }
        }
    }

    /// <summary>
    /// Inspects the incoming request and returns a PartialViewResult for Ajax requests and a full ViewResult for non-Ajax requests
    /// </summary>
    public class AjaxableViewResult : ActionResult
    {
        /// <summary>
        /// Determines the convention for looking up a PartialView for the incoming request. 
        /// The default convention looks for an underscore followed by the action name. 
        /// For example: _ContactUs.cshtml or _ContactUs.ascx
        /// </summary>
        public static Func<ControllerContext, string> AjaxViewNameConvention = context => "_" + context.RouteData.GetRequiredString("action");

        /// <summary>
        /// The view name for non-Ajax requests
        /// </summary>
        public string NonAjaxViewName { get; set; }

        /// <summary>
        /// The view name for Ajax requests
        /// </summary>
        public string AjaxViewName { get; set; }

        /// <summary>
        /// The model that is rendered to the view
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// Creates a new AjaxableViewResult using the default conventions
        /// </summary>
        public AjaxableViewResult()
            : this(null, null, null)
        {

        }

        /// <summary>
        /// Creates a new AjaxableViewResult using the default conventions with custom model data
        /// </summary>
        public AjaxableViewResult(object model)
            : this(null, null, model)
        {

        }

        /// <summary>
        /// Creates a new AjaxableViewResult with a specific ajax partial view
        /// </summary>
        public AjaxableViewResult(string ajaxViewName)
            : this(ajaxViewName, null)
        {

        }

        /// <summary>
        /// Creates a new AjaxableViewResult with a specific ajax partial view and model
        /// </summary>
        public AjaxableViewResult(string ajaxViewName, object model)
            : this(ajaxViewName, null, model)
        {

        }

        /// <summary>
        /// Creates a new AjaxableViewResult with a specific ajax view, non-ajax view, and model
        /// </summary>
        public AjaxableViewResult(string ajaxViewName, string defaultViewName, object model)
        {
            NonAjaxViewName = defaultViewName;
            AjaxViewName = ajaxViewName;
            Model = model;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            context.Controller.ViewData.Model = Model;

            if (context.HttpContext.Request.IsAjaxRequest())
            {
                var view = new PartialViewResult
                {
                    ViewName = GetAjaxViewName(context),
                    ViewData = context.Controller.ViewData
                };

                view.ExecuteResult(context);
            }
            else
            {
                var view = new ViewResult
                {
                    ViewName = GetViewName(context),
                    ViewData = context.Controller.ViewData
                };

                view.ExecuteResult(context);
            }

        }

        private string GetViewName(ControllerContext context)
        {
            return !string.IsNullOrEmpty(NonAjaxViewName) ? NonAjaxViewName : context.RouteData.GetRequiredString("action");
        }

        private string GetAjaxViewName(ControllerContext context)
        {
            return !string.IsNullOrEmpty(AjaxViewName) ? AjaxViewName : AjaxViewNameConvention(context);
        }
    }
}