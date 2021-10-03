import {Injectable} from '@angular/core';
import {ModalId} from "../enums/ModalId";

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private _shownModalsIds: string[] = [];

  constructor() {
  }

  show(id: ModalId): void {
    const modal = document.getElementById(id);

    if (!modal) {
      console.error(`Modal with id '${id}' not exist`);
      return;
    }

    modal!.classList.add('is-active');

    this._shownModalsIds.push(id);
  }

  hide(id: ModalId): void {
    const modal = document.getElementById(id);

    if (!modal) {
      console.error(`Modal with id '${id}' not exist`);
      return;
    }

    modal!.classList.remove('is-active');

    this._shownModalsIds = this._shownModalsIds.filter(modalId => modalId == id);
  }

  hideAll(): void {
    this._shownModalsIds.forEach(modalId => {
      document.getElementById(modalId)!.classList.remove('is-active');
    })
  }
}
