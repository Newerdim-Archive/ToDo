import {Injectable} from '@angular/core';
import {ModalId} from "../enums/ModalId";

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private _currentShown: ModalId | null = null;

  constructor() {
  }

  show(id: ModalId): void {
    this.hideCurrent();

    const modal = document.getElementById(id);

    if (!modal) {
      return;
    }

    modal.classList.add('is-active');

    this._currentShown = id;
  }

  hideCurrent(): void {
    if (!this._currentShown) {
      return;
    }

    const modal = document.getElementById(this._currentShown);

    if (!modal) {
      this._currentShown = null;
      return;
    }

    modal.classList.remove('is-active');

    this._currentShown = null;
  }
}
