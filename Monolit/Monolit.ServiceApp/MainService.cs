using System;
using System.Linq;
using ViageSoft.SystemServices.Applications;

namespace Monolit.ServiceApp
{
	public partial class MainService: WindowsServiceBase
	{
		public MainService()
			: base(new MainServiceApplication())
		{
			InitializeComponent();
		}
	}
}
