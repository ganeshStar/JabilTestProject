import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../_models';
import {  AuthenticationService, InventoryService, AlertService } from '../_services';
import { NgxSpinnerService } from "ngx-spinner";
import { ButtonRendererComponent } from '../_components';

@Component({ templateUrl: 'home.component.html' })

export class HomeComponent implements OnInit {
  currentUser: User;
  users = [];
  rowData = [];
  frameworkComponents: any;
  api: any;

  columnDefs = [
    { field: 'name', headerName: 'Name' },
    { field: 'price', headerName: 'Price', width: '100' },
    { field: 'quantity', headerName: 'Quantity', width: '120' },
    { field: 'createdDate', headerName: 'Created Date' },
    {
      headerName: 'Edit',
      cellRenderer: 'buttonRenderer',
      cellRendererParams: {
        onClick: this.onEditButtonClick.bind(this),
        label: 'Edit'
      },
    }
  ];

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private inventoryService: InventoryService,
    private alertService: AlertService,
    private spinner: NgxSpinnerService
  ) {
    this.currentUser = this.authenticationService.currentUserValue;
    this.frameworkComponents = {
      buttonRenderer: ButtonRendererComponent,
    }
  }

  ngOnInit() {
    this.getAllProduct();
  }

  onGridReady(params) {
    this.api = params.api;
  }
  onEditButtonClick(params) {
    this.router.navigate(['/addProduct'], { queryParams: { productId: params.data.id } });
  }

  private getAllProduct() {
    this.spinner.show();
    this.inventoryService.getAllProducts().
      subscribe(data => {
        this.rowData = data;
        this.spinner.hide();
      },
        error => {
          this.alertService.error(error);
          this.spinner.hide();
        });
  }
  addProduct() {
    this.router.navigate(['/addProduct']);
  }
}
