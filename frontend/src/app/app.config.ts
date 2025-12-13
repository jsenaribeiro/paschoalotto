import { provideHttpClient, withInterceptors } from "@angular/common/http";
import type { ApplicationConfig } from "@angular/core";
import { provideAnimationsAsync } from "@angular/platform-browser/animations/async";
import { provideRouter } from "@angular/router";
import Lara from "@primeng/themes/lara";
import { providePrimeNG } from "primeng/config";
import { routes } from "./app.routes";
import { loadingInterceptor } from "./shareds/loading/loading.interceptor";

export const appConfig: ApplicationConfig = {
	providers: [
		provideRouter(routes),
		provideHttpClient(withInterceptors([loadingInterceptor])),
		provideAnimationsAsync(),
		providePrimeNG({
			theme: {
				preset: Lara,
				options: {
					darkModeSelector: ".my-app-dark",
					cssLayer: {
						name: "primeng",
						order: "tailwind-base, primeng, tailwind-utilities",
					},
				},
			},
		}),
	],
};
