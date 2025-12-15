import { CommonModule } from "@angular/common";
import { Component, type OnInit } from "@angular/core";
import { RouterModule, RouterOutlet } from "@angular/router";
import { HeaderComponent, FooterComponent } from "./components";
import { LoadingComponent } from "./components/loading/loading";
@Component({
	standalone: true,
	selector: "app-root",
	styleUrls: ["./app.css"],
	templateUrl: "./app.html",
	imports: [
		CommonModule,
		RouterOutlet,
		RouterModule,
		LoadingComponent,
		HeaderComponent,
		FooterComponent,
	],
})
export class AppComponent implements OnInit {
	public currentRoute = "";

	public ngOnInit() {
		console.log("Sistema de Cobrança iniciado");
	}

	public isActiveRoute(route: string): boolean {
		return this.currentRoute === route;
	}
}
