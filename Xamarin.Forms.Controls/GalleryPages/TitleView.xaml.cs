using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Controls.GalleryPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TitleView : ContentPage
	{
		public TitleView(bool initialLoad)
		{
			InitializeComponent();

			if (initialLoad)
			{
				Device.BeginInvokeOnMainThread(() => navigationPage_Clicked(this, EventArgs.Empty));
			}

			//NavigationPage.SetTitleView(this, createGrid());
		}

		NavigationPage CreateNavigationPage()
		{
			return new NavigationPage(new TitleView(false) { Title = "Title" });
		}

		public Page GetPage()
		{
			return new MasterDetailPage()
			{
				Detail = CreateNavigationPage(),
				Master = new ContentPage() { Title = "Master" }
			};
		}

		private void masterDetailsPage_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage =
				new MasterDetailPage()
				{
					Detail = CreateNavigationPage(),
					Master = new ContentPage() { Title = "Master" }
				};

		}

		private void tabbedPage_Clicked(object sender, EventArgs e)
		{

			var page = new ContentPage() { Title = "other title page" };
			NavigationPage.SetTitleView(page, createGrid());

			App.Current.MainPage =
				new TabbedPage()
				{
					Children =
					{
						CreateNavigationPage(),
						new ContentPage(){ Title = "no title Page"},
						new NavigationPage(page),
					}
				};
		}

		private void navigationPage_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = CreateNavigationPage();
		}

		private void nextPage_Clicked(object sender, EventArgs e)
		{
			ContentPage page = null;
			page = new ContentPage()
			{
				Title = "second page",
				Content = new StackLayout()
				{
					Children =
					{
						new Button()
						{
							Text = "Toggle Back Button",
							Command = new Command(()=>
							{
								NavigationPage.SetHasBackButton(page, !NavigationPage.GetHasBackButton(page));
							})
						}
					}
				}};

			NavigationPage.SetHasBackButton(page, !NavigationPage.GetHasBackButton(page));
			NavigationPage.SetTitleView(page, createGrid());
			Navigation.PushAsync(page);
		}


		View createGrid()
		{
			var grid = new Grid
			{
				BackgroundColor = Color.Gray
			};

			grid.RowDefinitions.Add(new RowDefinition());
			grid.RowDefinitions.Add(new RowDefinition());
			grid.ColumnDefinitions.Add(new ColumnDefinition());
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

			var label = new Label { Text = "hello", HorizontalOptions = LayoutOptions.Start, BackgroundColor = Color.Yellow };
			var label2 = new Label { Text = "hello 2", HorizontalOptions = LayoutOptions.Start, BackgroundColor = Color.Yellow };
			grid.Children.Add(label);
			grid.Children.Add(label2);
			Grid.SetRow(label2, 1);

			var label3 = new Label { Text = "right aligned", HorizontalTextAlignment = TextAlignment.End };
			Grid.SetColumn(label3, 1);
			grid.Children.Add(label3);
			return grid;
		}


		private void titleIcon_Clicked(object sender, EventArgs e)
		{
			var titleIcon = NavigationPage.GetTitleIcon(this);

			if (titleIcon == null)
				NavigationPage.SetTitleIcon(this, "coffee.png");
			else
				NavigationPage.SetTitleIcon(this, null);

		}

		private void toggleLargeTitles_Clicked(object sender, EventArgs e)
		{
			var navPage = (NavigationPage)Navigation.NavigationStack.Last().Parent;
			navPage.On<iOS>().SetPrefersLargeTitles(!navPage.On<iOS>().PrefersLargeTitles());
		}

		private void backToGallery_Clicked(object sender, EventArgs e)
		{
			(App.Current as App).Reset();
		}

		private void toggleToolBarItem_Clicked(object sender, EventArgs e)
		{
			var page = Navigation.NavigationStack.Last();
			var items = page.ToolbarItems.Where(x => x.Order == ToolbarItemOrder.Primary).ToList();

			if (items.Any())
				foreach (var item in items)
					page.ToolbarItems.Remove(item);
			else
				page.ToolbarItems.Add(new ToolbarItem() { Text = "Save", Order = ToolbarItemOrder.Primary });
		}

		private void toggleSecondaryToolBarItem_Clicked(object sender, EventArgs e)
		{
			var page = Navigation.NavigationStack.Last();
			var items = page.ToolbarItems.Where(x => x.Order == ToolbarItemOrder.Secondary).ToList();

			if (items.Any())
				foreach (var item in items)
					page.ToolbarItems.Remove(item);
			else
				page.ToolbarItems.Add(new ToolbarItem() { Text = "Save", Order = ToolbarItemOrder.Secondary });
		}

		private void changeTitleView_Clicked(object sender, EventArgs e)
		{
			NavigationPage.SetTitleView(Navigation.NavigationStack.Last(), createGrid());
		}
	}
}