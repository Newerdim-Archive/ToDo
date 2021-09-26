import { Component, Input, OnInit } from "@angular/core";

@Component({
  selector: "app-google-button",
  templateUrl: "./google-button.component.html",
  styleUrls: ["./google-button.component.scss"],
})
export class GoogleButtonComponent implements OnInit {
  @Input() text = "";

  constructor() {}

  ngOnInit() {}
}
