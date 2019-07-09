using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface ITemplateService
    {
        /// <summary>
        /// Renders a template given the provided view model
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="filename">Filename of the template to render</param>
        /// <param name="viewModel">View model to use for rendering the template</param>
        /// <returns>Returns the rendered template content</returns>
        Task<string> RenderTemplateAsync<TViewModel>(string filename, TViewModel viewModel, Action<dynamic> viewBagInitialize = null);
    }
}