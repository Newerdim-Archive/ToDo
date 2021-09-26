import {Injectable} from '@angular/core';
import {ModalId} from "../types/ModalId";

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private shownModalsIds: string[] = [];

  constructor() {
  }

  show(id: ModalId): void {
    const modal = document.getElementById(id);

    if (!modal) {
      console.error(`Modal with id '${id}' not exist`);
      return;
    }

    modal!.classList.add('is-active');

    this.shownModalsIds.push(id);
  }

  hide(id: ModalId): void {
    const modal = document.getElementById(id);

    if (!modal) {
      console.error(`Modal with id '${id}' not exist`);
      return;
    }

    modal!.classList.remove('is-active');

    this.shownModalsIds = this.shownModalsIds.filter(modalId => modalId == id);
  }
}
