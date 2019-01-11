import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-setting-button",
  template: `
    <button id="settings" class="user-button">
      <img
        src="./assets/images/small-icons/settings.svg"
        class="nav-icon"
        aria-hidden="true"
      />
      Settings
    </button>
  `
})
export class SettingButtonComponent implements OnInit {
  constructor() {}

  ngOnInit() {}
}
