import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AppManagement } from '../app-management'; // Adjust path if needed

@Component({
  selector: 'app-app-registry',
  standalone: true,
  imports: [CommonModule, FormsModule], // Required for @for and ngModel
  templateUrl: './app-registry.html',
  styleUrl: './app-registry.scss',
})
export class AppRegistry implements OnInit {
  appService = inject(AppManagement);

  newApp = {
    clientName: '',
    redirectUri: '',
    allowedScopes: 'openid profile email',
    isActive: true
  };

  ngOnInit() {
    this.appService.loadApps();
  }

  onSubmit() {
    this.appService.registerApp(this.newApp).subscribe({
      next: () => {
        alert('Application Registered!');
        this.appService.loadApps(); // Refresh list
        this.newApp.clientName = ''; // Clear form
        this.newApp.redirectUri = '';
      },
      error: (err) => console.error('Registration failed', err)
    });
  }
}
