using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;
using NatickFantasyGM.Infrastructure.PlayerProjections;
using NatickFantasyGM.Core.PlayerProjections.Interfaces;

namespace NatickFantasyGM
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.RegisterBlazorMauiWebView()
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddBlazorWebView();
            //builder.Services.AddSingleton<IPlayerRepository>(new PlayerRepository());

			return builder.Build();
		}
	}
}