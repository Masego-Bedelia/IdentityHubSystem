import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../enviroments/environment';


@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-management.html',
  styleUrl: './user-management.scss',
})
export class UserManagement {
  private http = inject(HttpClient);

  email = '';
  roleName = '';
  selectedAppId = '';

  user = signal<any | null>(null);
  permissions = signal<any[]>([]);
  apps = signal<any[]>([]);

  ngOnInit() {
    this.http.get(`${environment.apiUrl}/Applications`)
      .subscribe((apps: any) => this.apps.set(apps));
  }

  loadUser() {
    this.http.get(`${environment.apiUrl}/Users/by-email?email=${this.email}`)
      .subscribe((res: any) => {
        this.user.set(res.user);
        this.permissions.set(res.permissions);
      });
  }

  assignRole() {
    this.http.post(`${environment.apiUrl}/Users/assign-role`, {
      userId: this.user()?.userId,
      appId: this.selectedAppId,
      roleName: this.roleName
    }).subscribe(() => {
      alert('Role assigned');
      this.loadUser();
    });
  }
}
