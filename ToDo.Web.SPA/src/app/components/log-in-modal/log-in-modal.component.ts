import {Component, OnInit} from '@angular/core';
import {ModalService} from "../../services/modal.service";

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
    this.modalService.hide("log-in-modal");
  }

  showSignUpModal(): void {
    this.modalService.hideAll();
    this.modalService.show('sign-up-modal');
  }
}
