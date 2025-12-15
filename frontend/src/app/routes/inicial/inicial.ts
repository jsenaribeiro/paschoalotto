import { Component, type OnInit, signal } from "@angular/core";
import type { CobrancaResumo, DashboardMetrics, Titulo } from "../../models";
import { pageModules } from '../../shareds';

@Component({ standalone: true,
				 imports: pageModules,
				 selector: "app-inicial",
				 styleUrls: ["./inicial.css"],
				 templateUrl: "./inicial.html" })
export class InicialComponent implements OnInit {
	public metrics = signal<DashboardMetrics | null>(null);
	public resumo = signal<CobrancaResumo | null>(null);
	public titulos = signal<Titulo[]>([]);
	public loading = signal(true);
	public chartData = {
		labels: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun"],
		datasets: [
			{
				label: "Títulos em Atraso",
				data: [12, 19, 8, 15, 22, 18],
				borderColor: "#0079B8",
				backgroundColor: "rgba(0, 121, 184, 0.1)",
				tension: 0.4,
			},
		],
	};

	public ngOnInit(): void {}
}
