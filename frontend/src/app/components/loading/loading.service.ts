import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
   private requestCount = 0;
   isLoading = signal<boolean>(false);

   public show() {
      console.log('LoadingService: show');
      this.requestCount++;
      this.isLoading.set(true);
   }

   public hide() {
      
      console.log('LoadingService: hide');
      this.requestCount--;
      if (this.requestCount <= 0) {
         this.requestCount = 0;
         this.isLoading.set(false);
      }
   }
}
