import {Component, OnInit} from '@angular/core';
import {ModalService} from "../../services/modal.service";
import {ModalId} from "../../enums/ModalId";

@Component({
  selector: 'app-log-in-modal',
  templateUrl: './log-in-modal.component.html',
  styleUrls: ['./log-in-modal.component.scss']
})
export class LogInModalComponent implements OnInit {

  constructor(private modalService: ModalService) {
  }

  ngOnInit(): void {
  }

  hideModal(): void {
    this.modalService.hide(ModalId.LogIn);
  }

  showSignUpModal(): void {
    this.modalService.hideAll();
    this.modalService.show(ModalId.SignUp);
  }
}
