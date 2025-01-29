import { MessageService } from 'primeng/api';

export function showSuccess(messageService: MessageService, detail: string) {
  messageService.add({
    severity: 'success',
    summary: 'Success',
    detail: detail,
  });
}

export function showError(messageService: MessageService, detail: string) {
  messageService.add({
    severity: 'error',
    summary: 'Error',
    detail: detail
  });
}
