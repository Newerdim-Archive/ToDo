import {Component, OnInit} from "@angular/core";
import {ModalService} from "../../services/modal.service";

@Component({
  selector: "app-home-nav",
  templateUrl: "./home-nav.component.html",
  styleUrls: ["./home-nav.component.scss"],
})
export class HomeNavComponent implements OnInit {
  isNavbarShown = false;

  constructor(private modalService: ModalService) {
  }

  ngOnInit(): void {
  }

  toggleNavbar(): void {
    this.isNavbarShown = !this.isNavbarShown;
  }

  showLogInModal() {
    this.modalService.show("log-in-modal");
  }

  showSignUpModal() {
    this.modalService.show("sign-up-modal");
  }
}
