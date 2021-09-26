import {Component, OnInit} from "@angular/core";
import {ModalService} from "../../services/modal.service";

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
    this.modalService.show("sign-up-modal");
  }
}
