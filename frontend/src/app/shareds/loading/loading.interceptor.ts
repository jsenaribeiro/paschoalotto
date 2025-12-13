import type { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoadingService } from './loading.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);
  
  // Opcional: Ignorar certas requisições se necessário
  // if (req.headers.has('X-Skip-Loading')) return next(req);

  loadingService.show();

  return next(req).pipe( finalize(() => loadingService.hide()) );
};
