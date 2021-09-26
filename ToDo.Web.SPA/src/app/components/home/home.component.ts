import {Component, OnInit} from "@angular/core";
import {ModalService} from "../../services/modal.service";
import {ModalId} from "../../enums/ModalId";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"],
})
export class HomeComponent implements OnInit {
  constructor(private modalService: ModalService) {
  }

  ngOnInit(): void {
  }

  showSignUpModal() {
    this.modalService.show(ModalId.SignUp);
  }
}
