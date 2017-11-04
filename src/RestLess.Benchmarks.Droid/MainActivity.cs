using Android.App;
using Android.Widget;
using Android.OS;
using Microcharts;
using SkiaSharp;
using Microcharts.Droid;
using Benchmarks;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;
using Android.Views;

namespace RestLess.Benchmarks.Droid
{
    [Activity(Label = "RestLess.Benchmarks.Droid", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private static readonly Dictionary<string, string> LibNameToColor = new Dictionary<string, string>
        {
            ["Refit"] = "#266489",
            ["RestLess"] = "#90D585",
        };

        private RecyclerView recyclerView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var button = this.FindViewById<Button>(Resource.Id.main_button);
            button.Click += this.Button_Click;

            this.recyclerView = this.FindViewById<RecyclerView>(Resource.Id.main_recyclerView);
        }

        private async void Button_Click(object sender, System.EventArgs e)
        {
            try
            {
                Benchmarker benchmarker = new Benchmarker();
                var results = await benchmarker.RunAsync();
                this.recyclerView.SetAdapter(new ChartsAdapter(results));
                this.recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private class ChartsAdapter : RecyclerView.Adapter
        {
            private readonly IReadOnlyList<Benchmark> benchmarks;

            public ChartsAdapter(IReadOnlyList<Benchmark> benchmarks)
            {
                this.benchmarks = benchmarks;
            }

            public override int ItemCount => this.benchmarks.Count;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                if (holder is ChartViewHolder chartViewHolder)
                {
                    var benchmark = this.benchmarks[position];
                    chartViewHolder.TextView.Text = benchmark.Title;

                    var entries = benchmark.Results
                                           .Select(x =>
                                            new Entry(x.ElapsedMilliseconds)
                                            {
                                                ValueLabel = x.ElapsedMilliseconds.ToString(),                                               
                                                Label = x.LibName,
                                                Color = SKColor.Parse(LibNameToColor[x.LibName])
                                            })
                                            .ToList();
                    var chart = new BarChart() { Entries = entries };
                    chartViewHolder.ChartView.Chart = chart;
                }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View itemView = Android.Views.LayoutInflater.From(parent.Context)
                                                            .Inflate(Resource.Layout.ChartCardView, parent, false);
                return new ChartViewHolder(itemView);
            }
        }

        private class ChartViewHolder : RecyclerView.ViewHolder
        {
            public ChartViewHolder(View itemView) : base(itemView)
            {
                this.TextView = itemView.FindViewById<TextView>(Resource.Id.chartcardview_textview);
                this.ChartView = itemView.FindViewById<ChartView>(Resource.Id.chartcardview_chartview);
            }

            public TextView TextView { get; }

            public ChartView ChartView { get; }
        }
    }
}

