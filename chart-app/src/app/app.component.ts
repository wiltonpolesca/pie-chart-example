import { Component, ViewChild } from '@angular/core';
import DatalabelsPlugin from 'chartjs-plugin-datalabels';
import { ChartConfiguration, ChartData, ChartEvent, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { PieChartData, PieChartService } from './services/pie-chart.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;

  /**
   *
   */
  constructor(private pieService: PieChartService) {
    this.pieService.data$.subscribe((value) => {
      this.updateChart(value);
    })
  }

  // Pie
  public pieChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'top',
      },
      datalabels: {
        formatter: (value, ctx) => {
          if (ctx.chart.data.labels) {
            return ctx.chart.data.labels[ctx.dataIndex];
          }
        },
      },
    }
  };
  public pieChartData: ChartData<'pie', number[], string | string[]> = {
    labels: [ ],
    datasets: [ {
      data: [ ]
    } ]
  };
  public pieChartType: ChartType = 'pie';
  public pieChartPlugins = [ DatalabelsPlugin ];

  updateChart(data: PieChartData): void {
    this.pieChartData.labels = data.labels;
    this.pieChartData.datasets[0].data = data.values;

    this.chart?.render();
  }
}
