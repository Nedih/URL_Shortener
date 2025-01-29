import { Injectable } from "@angular/core";
import { MessageService } from "primeng/api";

@Injectable({
  providedIn: 'root',
})
export class ToastService {

  constructor(
    private messageService: MessageService
  ) { }

  showSuccess(detail: string) {
    this.messageService.add({
      severity: 'success',
      summary: 'Success',
      detail: detail,
    });
  }

  showError(detail: string) {
    this.messageService.add({
      severity: 'danger',
      summary: 'Error',
      detail: detail
    });
  }
}
