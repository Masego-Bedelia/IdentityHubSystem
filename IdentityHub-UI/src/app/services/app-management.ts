import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { environment } from '../../enviroments/environment';

@Injectable({ providedIn: 'root' })
export class AppManagementService {
  private baseUrl = `${environment.apiUrl}/Applications`;
  
  // 2026 Standard: Using Signals for state management
  appList = signal<any[]>([]);

  constructor(private http: HttpClient) {}

  // Fetch all apps from the database
  getApplications() {
    this.http.get<any[]>(this.baseUrl).subscribe(data => {
      this.appList.set(data);
    });
  }

  // Register a new app
  registerApp(appData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/register`, appData);
  }
}
