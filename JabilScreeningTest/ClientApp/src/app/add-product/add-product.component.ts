import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertService, InventoryService, AuthenticationService } from '../_services';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})

export class AddProductComponent implements OnInit {
  addProductForm: FormGroup;
  loading = false;
  submitted = false;
  sub: any;
  productId: number = 0;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private inventoryService: InventoryService, private alertService: AlertService, private spinner: NgxSpinnerService) {
  }

  ngOnInit() {
    this.addProductForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      quantity: ['', Validators.required],
      price: ['', Validators.required],
      id: [0]
    });

    this.sub = this.route
      .queryParams
      .subscribe(params => {
        this.productId = +params['productId'] || 0;
        this.getProductToEdit();
      });
  }

  getProductToEdit() {
    this.spinner.show();
    this.inventoryService.getProductToEdit(this.productId)
      .subscribe(
        data => {
          this.spinner.hide();
          this.addProductForm.setValue({
            id: data.id,
            name: data.name,
            quantity: data.quantity,
            price: data.price
          });
        },
        error => {
          this.spinner.hide();
          this.alertService.error(error);
        });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  // convenience getter for easy access to form fields
  get f() { return this.addProductForm.controls; }

  onSubmit() {
    this.spinner.show();
    this.submitted = true;
    this.alertService.clear();
    //var qty = this.addProductForm.value.quantity;
    //if (qty == "" || qty == undefined || qty == null) {
    //  this.spinner.hide();
    //  return
    //}
    if (this.addProductForm.invalid) {
      this.spinner.hide();
      return;
    } else {
      this.loading = true;
      if (this.productId == 0) {
        this.inventoryService.addProduct(this.addProductForm.value)
          .subscribe(data => {
            this.alertService.success('Product added successfully.', true);
            this.spinner.hide();
            setTimeout(() => {
              this.alertService.clear();
              this.router.navigate(['/']);
            }, 600);
          },
            error => {
              this.alertService.error(error);
              this.loading = false;
              this.spinner.hide();
            });
      } else {
        this.inventoryService.updateProduct(this.addProductForm.value).subscribe(data => {
          this.alertService.success('Product updated successfully.', true);
          this.spinner.hide();
          setTimeout(() => {
            this.alertService.clear();
            this.router.navigate(['/']);
          }, 600);
        }, error => {
          this.alertService.error(error);
          this.loading = false;
          this.spinner.hide();
        });
      }
    }
  }

  validateNumber(evnt: any) {
    var value = evnt.target.value;
    var charC = (evnt.which) ? evnt.which : evnt.keyCode;
    if (charC == 46) {
      if (value.indexOf('.') === -1) {
        return true;
      } else {
        return false;
      }
    } else {
      if (charC > 31 && (charC < 48 || charC > 57))
        return false;
    }
    return true;
  }

  resetForm() {
    this.addProductForm.reset()
    this.submitted = false;
    if (this.productId !== 0) {
      this.getProductToEdit();
    }
  }

}
