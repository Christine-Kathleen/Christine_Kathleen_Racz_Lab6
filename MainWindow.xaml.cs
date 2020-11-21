using AutoLotModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Christine_Kathleen_Racz_Lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        //Default action is no action
        ActionState action = ActionState.Nothing;

        //DB model
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();

        //Viewsources
        CollectionViewSource customerViewSource;
        CollectionViewSource inventoryViewSource;
        CollectionViewSource customerOrdersViewSource;

        //Customers TAB bindings
        Binding firstNameTextBoxBinding = new Binding();
        Binding lastNameTextBoxBinding = new Binding();
        Binding CustomerIdTextBoxBinding = new Binding();

        //Inventory TAB bindings
        Binding colorTextBoxBinding = new Binding();
        Binding makeTextBoxBinding = new Binding();

        //Orders TAB bindings
        Binding cmbCustomersBinding = new Binding();
        Binding cmbInventoryBinding = new Binding();


        private void BindDataGrid()
        {
            var queryOrder = from ord in ctx.Orders
                             join cust in ctx.Customers on ord.CustId equals cust.CustId
                             join inv in ctx.Inventories on ord.CarId equals inv.CarId
                             select new
                             {
                                 ord.OrderId,
                                 ord.CarId,
                                 ord.CustId,
                                 cust.FirstName,
                                 cust.LastName,
                                 inv.Make,
                                 inv.Color
                             };
            customerOrdersViewSource.Source = queryOrder.ToList();
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            CustomersFinalAction(false);
            CustomersSetAllActions(true);

            firstNameTextBoxBinding.Path = new PropertyPath("FirstName");
            lastNameTextBoxBinding.Path = new PropertyPath("LastName");
            CustomerIdTextBoxBinding.Path = new PropertyPath("CustId");

            makeTextBoxBinding.Path = new PropertyPath("Make");
            colorTextBoxBinding.Path = new PropertyPath("Color");

            cmbCustomersBinding.Path = new PropertyPath("FirstName","LastName");
            cmbInventoryBinding.Path = new PropertyPath("Make", "Color");

            orderscmbCustomers.SetBinding(TextBox.TextProperty, cmbCustomersBinding);
            orderscmbInventory.SetBinding(TextBox.TextProperty, cmbInventoryBinding);

            inventorymakeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);
            inventorycolorTextBox.SetBinding(TextBox.TextProperty, colorTextBoxBinding);

            CustomersfirstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBoxBinding);
            CustomerslastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBoxBinding);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Load Colledtions
            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));

            inventoryViewSource.Source = ctx.Inventories.Local;
            customerViewSource.Source = ctx.Customers.Local;


            ctx.Customers.Load();
            ctx.Orders.Load();
            ctx.Inventories.Load();

            //Below comboboxes are on Orders Tab, maybe has to be checked???
            orderscmbCustomers.ItemsSource = ctx.Customers.Local;
            //cmbCustomers.DisplayMemberPath = "FirstName";
            orderscmbCustomers.SelectedValuePath = "CustId";
            orderscmbInventory.ItemsSource = ctx.Inventories.Local;
            //cmbInventory.DisplayMemberPath = "Make";
            orderscmbInventory.SelectedValuePath = "CarId";

            // Bind Specially customerOrdersViewSource
            //customerOrdersViewSource.Source = ctx.Orders.Local;
            //was under comboboxes in the code below
            BindDataGrid();
            customerViewSource.View.MoveCurrentToFirst();

        }

        private void CustomersTextBoxes(bool active)
        {
            CustomersfirstNameTextBox.IsEnabled = active;
            CustomerslastNameTextBox.IsEnabled = active;
        }
        /// <summary>
        /// this function 
        /// </summary>
        /// <param name="set">true Setbindings, false Clearbindings and set temporary values to textboxes</param>
        private void CustomersTextboxBindings(bool set)
        {
            if (set)
            {
                //Setbindings between grid and textboxes
                CustomersfirstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBoxBinding);
                CustomerslastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBoxBinding);
            }
            else
            {
                //save temporarly textboxes values
                string tempFirstName = CustomersfirstNameTextBox.Text;
                string tempLastName = CustomerslastNameTextBox.Text;
                

                //grid and textboxes bindings are cleared
                BindingOperations.ClearBinding(CustomersfirstNameTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(CustomerslastNameTextBox, TextBox.TextProperty);

                //Set back temporary values to textboxes, as bindigs cleared
                CustomersfirstNameTextBox.Text = tempFirstName;
                CustomerslastNameTextBox.Text = tempLastName;
            }
        }
        private void CustomersGridPrevNext(bool active)
        {
            customerDataGrid.IsEnabled = active;
            CustomersbtnPrevious.IsEnabled = active;
            CustomersbtnNext.IsEnabled = active;
        }
        private void CustomersFinalAction(bool active)
        {
            CustomersbtnSave.IsEnabled = active;
            CustomersbtnCancel.IsEnabled = active;
        }
        private void CustomersSetAllActions(bool active)
        {
            CustomersbtnNew.IsEnabled = active;
            CustomersbtnEdit.IsEnabled = active;
            CustomersbtnDelete.IsEnabled = active;
        }

        private void inventorySetAllActions(bool active)
        {
            inventorybtnNew.IsEnabled = active;
            inventorybtnEdit.IsEnabled = active;
            inventorybtnDelete.IsEnabled = active;
        }
        private void inventoryTextboxBindings(bool set)
        {
            if (set)
            {
                //Setbindings between grid and textboxes
                inventorymakeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);
                inventorycolorTextBox.SetBinding(TextBox.TextProperty, colorTextBoxBinding);
            }
            else
            {
                string tempColor = inventorycolorTextBox.Text.ToString();
                string tempMake = inventorymakeTextBox.Text.ToString();
                BindingOperations.ClearBinding(inventorycolorTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(inventorymakeTextBox, TextBox.TextProperty);
                inventorycolorTextBox.Text = tempColor;
                inventorymakeTextBox.Text = tempMake;
            }
        }
        private void inventoryTextBox(bool active)
        {
            inventorymakeTextBox.IsEnabled = active;
            inventorycolorTextBox.IsEnabled = active;
        }
        private void inventoryGridNextPrev(bool active)
        {
            inventoryDataGrid.IsEnabled = active;
            inventorybtnPervious.IsEnabled = active;
            inventorybtnNext.IsEnabled = active;
        }
        private void inventoryFinalActions(bool active)
        {
            inventorybtnSave.IsEnabled = active;
            inventorybtnCancel.IsEnabled = active;
        }

        private void ordersGridNextPrev(bool active)
        {
            ordersDataGrid.IsEnabled = active;
            ordersbtnPervious.IsEnabled = active;
            ordersbtnNext.IsEnabled = active;
        }
        private void ordersFinalActions(bool active)
        {
            ordersbtnSave.IsEnabled = active;
            ordersbtnCancel.IsEnabled = active;
        }
        private void ordersSetAllActions(bool active)
        {
            ordersbtnNew.IsEnabled = active;
            ordersbtnEdit.IsEnabled = active;
            ordersbtnDelete.IsEnabled = active;
        }
        private void ordersTextboxBindings(bool set)
        {
            if (set)
            {
                //Setbindings between grid and textboxes
                orderscmbCustomers.SetBinding(TextBox.TextProperty, cmbCustomersBinding);
                orderscmbInventory.SetBinding(TextBox.TextProperty, cmbInventoryBinding);
            }
            else
            {
                string tempCustomers = orderscmbCustomers.Text.ToString();
                string tempInventory = orderscmbInventory.Text.ToString();
                BindingOperations.ClearBinding(orderscmbCustomers, ComboBox.TextProperty);
                BindingOperations.ClearBinding(orderscmbInventory, ComboBox.TextProperty);
                orderscmbCustomers.Text = tempCustomers;
                orderscmbInventory.Text = tempInventory;
            }
        }
        private void ordersComboBox(bool active)
        {
            orderscmbCustomers.IsEnabled = active;
            orderscmbInventory.IsEnabled = active;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Customer object set to null
            Customer customer = null;
            
            switch (action)
            {
                case ActionState.New:
                    ////SetValidationBinding();
                    try
                    {
                        //instantiem Customer entity
                        customer = new Customer()
                        {
                            FirstName = CustomersfirstNameTextBox.Text.Trim(),
                            LastName = CustomerslastNameTextBox.Text.Trim()
                        };
                        //adaugam entitatea nou creata in context
                        ctx.Customers.Add(customer);
                        customerViewSource.View.Refresh();
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                    //using System.Data;
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }



                    break;
                case ActionState.Edit:
                    try
                    {
                        
                        customer = (Customer)customerDataGrid.SelectedItem;
                        customer.FirstName = CustomersfirstNameTextBox.Text.Trim();
                        customer.LastName = CustomerslastNameTextBox.Text.Trim();
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    customerViewSource.View.Refresh();
                    // pozitionarea pe item-ul curent
                    customerViewSource.View.MoveCurrentTo(customer);



                    //Validation is ON for textboxes
                    //SetValidationBinding();

                    break;
                case ActionState.Delete:
                    try
                    {
                        if(!(customerDataGrid.SelectedItem is Customer))
                        {
                            MessageBox.Show("Not a customer");
                            return;
                        }
                        customer = (Customer)customerDataGrid.SelectedItem;
                        ctx.Customers.Remove(customer);
                        ctx.SaveChanges();
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    customerViewSource.View.Refresh();


                    break;
                default:
                    action = ActionState.Nothing;
                    break;
            }

            CustomersSetAllActions(true);
            CustomersFinalAction(false);
            CustomersGridPrevNext(true);
            CustomersTextBoxes(false);

            CustomersTextboxBindings(true);
            //Setbindings between grid and textboxes
            custIdTextBox.SetBinding(TextBox.TextProperty, CustomerIdTextBoxBinding);


        }

        //checked
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (!e.Handled && sender is Button && ((Button)sender).DataContext is CollectionViewSource)
            {
                //((CollectionViewSource)((Button)sender).DataContext).View.MoveCurrentToNext();
                if (((CollectionViewSource)((Button)sender).DataContext).View.MoveCurrentToNext() == false)
                {
                    ((CollectionViewSource)((Button)sender).DataContext).View.MoveCurrentToPrevious();
                }
            }
            if (((CollectionViewSource)((Button)sender).DataContext) == customerViewSource)
            {
                customerDataGrid.ScrollIntoView(((CollectionViewSource)((Button)sender).DataContext).View.CurrentItem);
            }
        }

        //checked
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            //customerDataGrid.SelectedIndex--;
            if (!e.Handled && sender is Button && ((Button)sender).DataContext is CollectionViewSource)
            {
                if (((CollectionViewSource)((Button)sender).DataContext).View.CurrentPosition > 0)
                    ((CollectionViewSource)((Button)sender).DataContext).View.MoveCurrentToPrevious();
            }
            if (((CollectionViewSource)((Button)sender).DataContext)== customerViewSource)
            {
                customerDataGrid.ScrollIntoView(((CollectionViewSource)((Button)sender).DataContext).View.CurrentItem);
            }
        }

        //checked, looks ok
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            //New action set
            action = ActionState.New;

            //all actions deactivated
            CustomersSetAllActions(false);

            //all final action activated
            CustomersFinalAction(true);

            //Grid and prev,next are disabled
            CustomersGridPrevNext(false);

            //Clear bindings as we dont wanna modify the selected item in the grid
            CustomersTextboxBindings(false);

            //Textboxes enabled to enter new values
            CustomersTextBoxes(true);

            //textboxes set to empty
            CustomersfirstNameTextBox.Text = "";
            CustomerslastNameTextBox.Text = "";

            //set focus to FirstnameTextbox
            Keyboard.Focus(CustomersfirstNameTextBox);





            //grid and textboxes bindings are cleared
            BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);

            //Set back temporary values to textboxes, as bindigs cleared
            custIdTextBox.Text = "";


            SetValidationBinding();

        }

        //checked, looks ok
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

            //Action Edit set
            action = ActionState.Edit;

            //all actions deactivated
            CustomersSetAllActions(false);

            //all final actions activated
            CustomersFinalAction(true);

            //Grid and prev,next buttons disabled
            CustomersGridPrevNext(false);

            //Textboxes are enabled to be able to edit values
            CustomersTextBoxes(true);

            
            CustomersTextboxBindings(false);

            //Set focus to Firstname
            Keyboard.Focus(CustomersfirstNameTextBox);

            //Validation is ON for textboxes
            //SetValidationBinding();
        }

        //Checked, there is TODO
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //TODO somehow should be checked that any grid row has been selected

            // Delete action set
            action = ActionState.Delete;

            //Actions deactivated
            CustomersSetAllActions(false);

            //FinishActions activated
            CustomersFinalAction(true);


            //GRid,PRev,NExt deactivated
            CustomersGridPrevNext(false);

            CustomersTextboxBindings(false);



            //was disabled when we checked, lets see its neccesary
            //Should be disabled as user may modify the selected customer, names altough its assigned for deletion
            CustomersTextBoxes(false);
        }

        //Checked, looks ok
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //All previous action cancelled
            action = ActionState.Nothing;

            //No save or Cancel possible as no active action
            CustomersFinalAction(false);

            //All possible action are enabled
            CustomersSetAllActions(true);

            //grid and previous, next function are enabled
            CustomersGridPrevNext(true);

            //First and Lastname cant be edited as there are no active action
            CustomersTextBoxes(false);

            //Binding between grid and textboxes are enabled
            CustomersTextboxBindings(true);


            //Setbindings between grid and textboxes
            custIdTextBox.SetBinding(TextBox.TextProperty, CustomerIdTextBoxBinding);
        }

        private void SetValidationBinding()
        {
           // Binding firstNameValidationBinding = new Binding();
           // firstNameValidationBinding.Source = null;
           // firstNameValidationBinding.Path = new PropertyPath("FirstName");
           // firstNameValidationBinding.NotifyOnValidationError = true;
           // firstNameValidationBinding.Mode = BindingMode.TwoWay;
           // firstNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.Default;
           // //string required (cant be empty)
           // firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
           // CustomersfirstNameTextBox.SetBinding(TextBox.TextProperty, firstNameValidationBinding);
           // Binding lastNameValidationBinding = new Binding();
           //// lastNameValidationBinding.Source = customerViewSource;
           // lastNameValidationBinding.Path = new PropertyPath("LastName");
           // lastNameValidationBinding.NotifyOnValidationError = true;
           // lastNameValidationBinding.Mode = BindingMode.TwoWay;
           // lastNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
           // //string min length validator (min 3 characters)
           // lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
           // CustomerslastNameTextBox.SetBinding(TextBox.TextProperty, lastNameValidationBinding); //setare binding nou
        }

        private void btnNew1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;

            inventorySetAllActions(false);
            inventoryFinalActions(true);
            inventoryGridNextPrev(false);
            inventoryTextBox(true);
            inventoryTextboxBindings(false);
            
            inventorycolorTextBox.Text = "";
            inventorymakeTextBox.Text = "";
            Keyboard.Focus(inventorycolorTextBox);
        }

        private void btnEdit1_Click(object sender, RoutedEventArgs e)
        {

            action = ActionState.Edit;

            inventorySetAllActions(false);
            inventoryFinalActions(true);
            inventoryGridNextPrev(false);
            inventoryTextBox(true);
            inventoryTextboxBindings(false);
            
            Keyboard.Focus(inventorycolorTextBox);
            //SetValidationBinding();
        }

        private void btnDelete1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;

            inventorySetAllActions(false);
            inventoryFinalActions(true);
            inventoryGridNextPrev(false);
            inventoryTextBox(true);
            inventoryTextboxBindings(false);
        }

        private void btnSave1_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = null;
            switch (action)
            {
                case ActionState.New:
                    try
                    {

                        inventory = new Inventory()
                        {
                            Make = inventorymakeTextBox.Text.Trim(),
                            Color = inventorycolorTextBox.Text.Trim()
                        };
                        //adaugam entitatea nou creata in context
                        ctx.Inventories.Add(inventory);
                        customerViewSource.View.Refresh();
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                    //using System.Data;
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case ActionState.Edit:
                    {
                        try
                        {
                            inventory = (Inventory)inventoryDataGrid.SelectedItem;
                            inventory.Make = inventorymakeTextBox.Text.Trim();
                            inventory.Color = inventorycolorTextBox.Text.Trim();
                            //salvam modificarile
                            ctx.SaveChanges();
                        }
                        catch (DataException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        customerViewSource.View.Refresh();
                        // pozitionarea pe item-ul curent
                        inventoryViewSource.View.MoveCurrentTo(inventory);

                        inventoryTextboxBindings(false);
                        break;
                    }
                case ActionState.Delete:
                    {
                        try
                        {
                            inventory = (Inventory)customerDataGrid.SelectedItem;
                            ctx.Inventories.Remove(inventory);
                            ctx.SaveChanges();
                        }
                        catch (DataException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        customerViewSource.View.Refresh();

                        inventoryTextboxBindings(true);
                    }
                    break;

                default:
                    action = ActionState.Nothing;
                    break;       
            }
            inventorySetAllActions(true);
            inventoryFinalActions(false);
            inventoryGridNextPrev(true);
            inventoryTextBox(false);

        }
        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            inventorySetAllActions(true);
            inventoryFinalActions(false);
            inventoryGridNextPrev(true);
            inventoryTextBox(false);
            inventoryTextboxBindings(true);
        }

        private void btnNew0_Click(object sender, RoutedEventArgs e)
        {

            action = ActionState.New;
            ordersSetAllActions(false);
            ordersFinalActions(true);
            ordersGridNextPrev(false);
            ordersComboBox(true);

            ordersTextboxBindings(false);

            orderscmbCustomers.Text = "";
            orderscmbInventory.Text = "";
            Keyboard.Focus(orderscmbCustomers);
        }

        private void btnEdit0_Click(object sender, RoutedEventArgs e)
        {

            action = ActionState.Edit;

            ordersSetAllActions(false);
            ordersFinalActions(true);
            ordersGridNextPrev(false);
            ordersComboBox(true);

            ordersTextboxBindings(false);

            //SetValidationBinding();

        }

        private void btnSave0_Click(object sender, RoutedEventArgs e)
        {



            Order order = null;

            switch (action)
            {
                case ActionState.New:
                   // //SetValidationBinding();
                    try
                    {

                        Customer customer = (Customer)orderscmbCustomers.SelectedItem;
                        Inventory inventory = (Inventory)orderscmbInventory.SelectedItem;

                        //instantiem Order entity
                        order = new Order()
                        {

                            CustId = customer.CustId,
                            CarId = inventory.CarId
                        };
                        //adaugam entitatea nou creata in context
                        ctx.Orders.Add(order);
                        customerOrdersViewSource.View.Refresh();
                        ctx.SaveChanges();
                    }
                    //using System.Data;
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case ActionState.Edit:

                    try
                    {
                        Customer customer = (Customer)orderscmbCustomers.SelectedItem;
                        Inventory inventory = (Inventory)orderscmbInventory.SelectedItem;
                        order = (Order)ordersDataGrid.SelectedItem;
                        order.CustId = customer.CustId;
                        order.CarId = inventory.CarId;

                        //instantiem Order entity
                        order = new Order()
                        {

                            CustId = customer.CustId,
                            CarId = inventory.CarId
                        };
                        //adaugam entitatea nou creata in context
                        ctx.Orders.Add(order);
                        customerOrdersViewSource.View.Refresh();
                        int curr_id = order.OrderId;
                        var editedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                        if (editedOrder != null)
                        {
                            editedOrder.CustId = Int32.Parse(orderscmbCustomers.SelectedValue.ToString());
                            editedOrder.CarId = Convert.ToInt32(orderscmbInventory.SelectedValue.ToString());
                            ctx.SaveChanges();
                        }
                        order = (Order)ordersDataGrid.SelectedItem;
                        customer.FirstName = CustomersfirstNameTextBox.Text.Trim();
                        customer.LastName = CustomerslastNameTextBox.Text.Trim();
                        ctx.SaveChanges();
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    

                    // pozitionarea pe item-ul curent
                    customerOrdersViewSource.View.Refresh();
                    customerOrdersViewSource.View.MoveCurrentTo(order);

                    CustomersTextboxBindings(true);

                    BindDataGrid();

                    break;
                case ActionState.Delete:
                    dynamic selectedOrder = ordersDataGrid.SelectedItem;
                    try
                    {
                        Customer customer = (Customer)orderscmbCustomers.SelectedItem;
                        Inventory inventory = (Inventory)orderscmbInventory.SelectedItem;
                        //instantiem Order entity
                        order = new Order()
                        {

                            CustId = customer.CustId,
                            CarId = inventory.CarId
                        };
                        //adaugam entitatea nou creata in context
                        ctx.Orders.Add(order);
                        customerOrdersViewSource.View.Refresh();
                        int curr_id = ((Order)ordersDataGrid.SelectedItem).OrderId;
                        var deletedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                        if (deletedOrder != null)
                        {
                            ctx.Orders.Remove(deletedOrder);
                            ctx.SaveChanges();
                            MessageBox.Show("Order Deleted Successfully", "Message");
                            BindDataGrid();
                        }
                        order = (Order)ordersDataGrid.SelectedItem;
                        ctx.Orders.Remove(order);
                        ctx.SaveChanges();
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    customerOrdersViewSource.View.Refresh();
                    CustomersTextboxBindings(true);
                    break;
                default:
                    action = ActionState.Nothing;
                    break;
            }
            ordersSetAllActions(true);
            ordersFinalActions(false);
            ordersGridNextPrev(true);
            ordersComboBox(true);
}

        private void btnDelete0_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;

            ordersSetAllActions(false);
            ordersFinalActions(true);
            ordersGridNextPrev(false);
            ordersComboBox(false);

            ordersTextboxBindings(false);
        }

        private void btnCancel0_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            ordersSetAllActions(true);
            ordersFinalActions(false);
            ordersGridNextPrev(true);
            ordersComboBox(false);

            ordersTextboxBindings(true);
        }




    }
}

