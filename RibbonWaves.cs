#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class RibbonWaves : Indicator
	{
		private MovingAverageRibbon MovingAverageRibbon1;
		private ninZaKarthikDynamicRSI ninZaKarthikDynamicRSI1;
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "RibbonWaves";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= true;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				BullishColor					= Brushes.DodgerBlue;
				BearishColor					= Brushes.Red;
				BuySound					= @"smsAlert1.Wav";
				SellSOund					= @"smsAlert2.Wav";
				AlertOn						= true;
			}
			else if (State == State.Configure)
			{
			}
			else if (State == State.DataLoaded)
			{				
				MovingAverageRibbon1				= MovingAverageRibbon(Close, RibbonMAType.Exponential, 18, 18);
				ninZaKarthikDynamicRSI1				= ninZaKarthikDynamicRSI(Close, 14, 50, 15, 15, ninZaKarthikDynamicRSI_MAType.WMA);
			}
		}

		protected override void OnBarUpdate()
		{
			if (BarsInProgress != 0) 
				return;

			if (CurrentBars[0] < 0)
				return;

			// Bullish Trend
			if ((MovingAverageRibbon1.MovingAverage1[0] > MovingAverageRibbon1.MovingAverage8[0]) 
				&& (MovingAverageRibbon1.MovingAverage1[0] > MovingAverageRibbon1.MovingAverage2[0]) 
				&& (MovingAverageRibbon1.MovingAverage2[0] > MovingAverageRibbon1.MovingAverage3[0]) )
				{
					// bullish Pullback
					if (Close[0] < MovingAverageRibbon1.MovingAverage1[0] && Close[0] > Open[0] ) 
					{						
						CandleOutlineBrush =  BullishColor;
						BarBrush = BullishColor;							
						
						if (ninZaKarthikDynamicRSI1.DynamicRSI[0] < ninZaKarthikDynamicRSI1.LowLine[0])
						{ 
							Draw.TriangleUp(this, "bullsetup"+CurrentBar, false, 0, Low[0] - 2 * TickSize, BullishColor);
							if ( AlertOn ) {
								Alert("myAlert"+CurrentBar, Priority.High, "Buy Signal", NinjaTrader.Core.Globals.InstallDir+@"\sounds\"+ BuySound,10, Brushes.Black, Brushes.Yellow);
							}
						}
					}
				}
			
			// Bearish Trend
			if ((MovingAverageRibbon1.MovingAverage1[0] < MovingAverageRibbon1.MovingAverage8[0]) 
				&& (MovingAverageRibbon1.MovingAverage1[0] < MovingAverageRibbon1.MovingAverage2[0]) 
				&& (MovingAverageRibbon1.MovingAverage2[0] < MovingAverageRibbon1.MovingAverage3[0]) ) 
				{ 
					// bearish Pullback
					if (Close[0] > MovingAverageRibbon1.MovingAverage1[0] && Close[0] < Open[0] ) 
					{						
						CandleOutlineBrush =  BearishColor;
						BarBrush = BearishColor;							
						
						if (ninZaKarthikDynamicRSI1.DynamicRSI[0] > ninZaKarthikDynamicRSI1.HighLine[0])
						{ 
							Draw.TriangleDown(this, "bearishsetup"+CurrentBar, false, 0, High[0] + 2 * TickSize, BearishColor);
							if ( AlertOn ) {
								Alert("myAlert"+CurrentBar, Priority.High, "Sell Signal", NinjaTrader.Core.Globals.InstallDir+@"\sounds\"+ SellSOund,10, Brushes.Black, Brushes.Yellow);
							}
						}
					}
				}
		}

		#region Properties
		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="BullishColor", Order=1, GroupName="Parameters")]
		public Brush BullishColor
		{ get; set; }

		[Browsable(false)]
		public string BullishColorSerializable
		{
			get { return Serialize.BrushToString(BullishColor); }
			set { BullishColor = Serialize.StringToBrush(value); }
		}			

		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="BearishColor", Order=2, GroupName="Parameters")]
		public Brush BearishColor
		{ get; set; }

		[Browsable(false)]
		public string BearishColorSerializable
		{
			get { return Serialize.BrushToString(BearishColor); }
			set { BearishColor = Serialize.StringToBrush(value); }
		}			

		[NinjaScriptProperty]
		[Display(Name="BuySound", Order=3, GroupName="Parameters")]
		public string BuySound
		{ get; set; }

		[NinjaScriptProperty]
		[Display(Name="SellSOund", Order=4, GroupName="Parameters")]
		public string SellSOund
		{ get; set; }
		 
		[NinjaScriptProperty]
		[Display(Name="Alerts On", Description="AlertOn", Order=5, GroupName="Parameters")]
		public bool AlertOn
		{ get; set; }
		
		#endregion

	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private RibbonWaves[] cacheRibbonWaves;
		public RibbonWaves RibbonWaves(Brush bullishColor, Brush bearishColor, string buySound, string sellSOund, bool alertOn)
		{
			return RibbonWaves(Input, bullishColor, bearishColor, buySound, sellSOund, alertOn);
		}

		public RibbonWaves RibbonWaves(ISeries<double> input, Brush bullishColor, Brush bearishColor, string buySound, string sellSOund, bool alertOn)
		{
			if (cacheRibbonWaves != null)
				for (int idx = 0; idx < cacheRibbonWaves.Length; idx++)
					if (cacheRibbonWaves[idx] != null && cacheRibbonWaves[idx].BullishColor == bullishColor && cacheRibbonWaves[idx].BearishColor == bearishColor && cacheRibbonWaves[idx].BuySound == buySound && cacheRibbonWaves[idx].SellSOund == sellSOund && cacheRibbonWaves[idx].AlertOn == alertOn && cacheRibbonWaves[idx].EqualsInput(input))
						return cacheRibbonWaves[idx];
			return CacheIndicator<RibbonWaves>(new RibbonWaves(){ BullishColor = bullishColor, BearishColor = bearishColor, BuySound = buySound, SellSOund = sellSOund, AlertOn = alertOn }, input, ref cacheRibbonWaves);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.RibbonWaves RibbonWaves(Brush bullishColor, Brush bearishColor, string buySound, string sellSOund, bool alertOn)
		{
			return indicator.RibbonWaves(Input, bullishColor, bearishColor, buySound, sellSOund, alertOn);
		}

		public Indicators.RibbonWaves RibbonWaves(ISeries<double> input , Brush bullishColor, Brush bearishColor, string buySound, string sellSOund, bool alertOn)
		{
			return indicator.RibbonWaves(input, bullishColor, bearishColor, buySound, sellSOund, alertOn);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.RibbonWaves RibbonWaves(Brush bullishColor, Brush bearishColor, string buySound, string sellSOund, bool alertOn)
		{
			return indicator.RibbonWaves(Input, bullishColor, bearishColor, buySound, sellSOund, alertOn);
		}

		public Indicators.RibbonWaves RibbonWaves(ISeries<double> input , Brush bullishColor, Brush bearishColor, string buySound, string sellSOund, bool alertOn)
		{
			return indicator.RibbonWaves(input, bullishColor, bearishColor, buySound, sellSOund, alertOn);
		}
	}
}

#endregion
