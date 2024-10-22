import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
  imports: [CommonModule]
})
export class UserProfileComponent implements OnInit {
  userData: any;  // This will store the user's personal data

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    // Automatically request and load the user's data when the component loads
    this.requestMyData();
  }

  // Method to request personal data
  requestMyData(): void {
    this.userService.accessMyData().subscribe(data => {
      this.userData = data;  // Store the received data
    });
  }

  // Method to download the personal data as a JSON file
  downloadMyData(): void {
    this.userService.accessMyData().subscribe(data => {
      const blob = new Blob([JSON.stringify(data, null, 2)], { type: 'application/json' });
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = 'my_data.json';
      link.click();
    });
  }
}
