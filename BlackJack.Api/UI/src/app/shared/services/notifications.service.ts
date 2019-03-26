import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {

  constructor(private toastr: ToastrService) {
  }

  public showNotifications(message: string): void {
    this.toastr.success(message);
  }

  public showError(message: string): void {
    this.toastr.error(message);
  }
}
