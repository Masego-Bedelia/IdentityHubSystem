import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AppManagement {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:44331/api/Applications';

  // 2026 Signal to store state
  appList = signal<any[]>([]);

  // Load apps from SQL
  loadApps() {
    this.http.get<any[]>(this.apiUrl).subscribe(data => {
      this.appList.set(data);
    });
  }

  // Register new app
  registerApp(appData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, appData);
  }
}
