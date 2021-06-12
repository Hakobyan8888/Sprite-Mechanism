using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GridSprite.CustomControls
{
    public class TableGridView : Control
    {
        private bool fromUI;
        private Grid _grid;

        public TableGridView()
        {
            DefaultStyleKey = typeof(TableGridView);
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(object), typeof(TableGridView), new PropertyMetadata(null));

        public int RowCount
        {
            get { return (int)GetValue(RowCountProperty); }
            set
            {
                SetValue(RowCountProperty, value);
                if (!fromUI)
                    InitRowsAndColumns();
            }
        }

        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.Register("RowCount", typeof(int), typeof(TableGridView), new PropertyMetadata(0));

        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set
            {
                SetValue(ColumnCountProperty, value);
                if (!fromUI)
                    InitRowsAndColumns();
            }
        }

        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.Register("ColumnCount", typeof(int), typeof(TableGridView), new PropertyMetadata(0));


        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set
            {
                SetValue(ItemWidthProperty, value);
                SetItemWidthHeight();
            }
        }

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(TableGridView), new PropertyMetadata(0));

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set
            {
                SetValue(ItemHeightProperty, value);
                SetItemWidthHeight();

            }
        }

        private void SetItemWidthHeight()
        {
            if (_grid == null) return;
            for (int i = 1; i < _grid?.RowDefinitions.Count; i++)
            {
                if (i % 2 == 1)
                {
                    _grid.RowDefinitions[i].Height = new GridLength(3, GridUnitType.Pixel);
                }
                else
                    _grid.RowDefinitions[i].Height = new GridLength(ItemHeight, GridUnitType.Pixel);
            }

            for (int i = 1; i < _grid?.ColumnDefinitions.Count; i++)
            {
                if (i % 2 == 1)
                {
                    _grid.ColumnDefinitions[i].Width = new GridLength(3, GridUnitType.Pixel);
                }
                else
                    _grid.ColumnDefinitions[i].Width = new GridLength(ItemWidth, GridUnitType.Pixel);
            }
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(TableGridView), new PropertyMetadata(0));

        public Path GetPath(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Path path)
                    return path;
                else
                {
                    var obj = GetPath(child);
                    if (obj is Path p)
                        return p;
                }
            }
            return null;
        }

        public void InitItems()
        {
            for (int x = 1; x < _grid.ColumnDefinitions.Count - 1; x++)
            {
                if (x % 2 == 0)
                {
                    for (int y = 1; y < _grid.RowDefinitions.Count - 1; y++)
                    {
                        if (y % 2 == 0)
                        {
                            var rectangle = new RectangleGeometry
                            {
                                Rect = new Rect(new Point(0, 0), new Point(100, 100)),
                            };
                            //var path = new Path
                            //{
                            //    Data = rectangle,
                            //    Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),
                            //};
                            //var grid = new Grid()
                            //{
                            //    BorderThickness = new Thickness(7),
                            //    BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),
                            //};
                            var a = new GridCell();
                            a.SetValue(Grid.RowProperty, y);
                            a.SetValue(Grid.ColumnProperty, x);
                            _grid.Children.Add(a);
                            var p = GetPath(a);
                            p.Data = rectangle;
                        }
                    }
                }
            }


            //if (_grid == null) return;
            //_grid.Children.Clear();
            //if (Items is System.Collections.IList list)
            //{
            //    int i = 0;
            //    for (int x = 1; x <= ColumnCount * 2 + 1; x++)
            //    {
            //        if (x % 2 == 1)
            //        {
            //            var border = new Border
            //            {
            //                BorderThickness = new Thickness(3),
            //                Width = 3,
            //                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255)),
            //                ManipulationMode = ManipulationModes.TranslateX
            //            };
            //            border.ManipulationDelta += Border_ManipulationDeltaColumn;
            //            Grid.SetRow(border, 1);
            //            Grid.SetRowSpan(border, RowCount * 2 + 2);
            //            Grid.SetColumn(border, x);
            //            _grid.Children.Add(border);
            //            continue;
            //        }
            //        for (int y = 1; y <= RowCount * 2 + 1; y++)
            //        {
            //            if (y % 2 == 1)
            //            {
            //                var border = new Border
            //                {
            //                    BorderThickness = new Thickness(3),
            //                    Height = 3,
            //                    BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255)),
            //                    ManipulationMode = ManipulationModes.TranslateY
            //                };
            //                border.PointerEntered += Border_PointerEnteredRow;
            //                border.PointerExited += Border_PointerExited;
            //                border.ManipulationDelta += Border_ManipulationDeltaRow;
            //                Grid.SetColumn(border, 1);
            //                Grid.SetColumnSpan(border, ColumnCount * 2 + 1);
            //                Grid.SetRow(border, y);
            //                _grid.Children.Add(border);
            //            }
            //            else
            //            {
            //                if (i < list.Count)
            //                {
            //                    var item = list[i] as FrameworkElement;
            //                    _grid.Children.Add(item);
            //                    Grid.SetColumn(item, x);
            //                    Grid.SetRow(item, y);
            //                    i++;
            //                }
            //            }
            //        }
            //    }
            //}
        }

        public void InitRowsAndColumns()
        {
            if (_grid == null) return;
            _grid.RowDefinitions.Clear();
            _grid.ColumnDefinitions.Clear();
            _grid.Children.Clear();
            _grid.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(3, GridUnitType.Pixel)
            });
            _grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(3, GridUnitType.Pixel)
            });
            for (int i = 1; i <= RowCount * 2 + 1; i++)
            {
                _grid.RowDefinitions.Add(new RowDefinition());

                if (i % 2 == 1)
                {
                    var border = new Border
                    {
                        Name = "RowBorder",
                        BorderThickness = new Thickness(3),
                        Height = 3,
                        BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255)),
                        ManipulationMode = ManipulationModes.TranslateY
                    };
                    border.PointerEntered += Border_PointerEnteredRow;
                    border.PointerExited += Border_PointerExited;
                    border.ManipulationDelta += Border_ManipulationDeltaRow;
                    Grid.SetColumn(border, 1);
                    Grid.SetColumnSpan(border, ColumnCount * 2 + 1);
                    Grid.SetRow(border, i);
                    _grid.Children.Add(border);
                }
            }
            for (int i = 1; i <= ColumnCount * 2 + 1; i++)
            {
                _grid.ColumnDefinitions.Add(new ColumnDefinition() { });

                if (i % 2 == 1)
                {
                    var border = new Border
                    {
                        Name = "ColumnBorder",
                        BorderThickness = new Thickness(3),
                        Width = 3,
                        BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255)),
                        ManipulationMode = ManipulationModes.TranslateX
                    };
                    border.ManipulationDelta += Border_ManipulationDeltaColumn;
                    border.PointerEntered += Border_PointerEnteredColumn;
                    border.PointerExited += Border_PointerExited;
                    Grid.SetRow(border, 1);
                    Grid.SetRowSpan(border, RowCount * 2 + 2);
                    Grid.SetColumn(border, i);
                    _grid.Children.Add(border);
                }
            }
            SetItemWidthHeight();
            InitItems();
        }

        protected override void OnApplyTemplate()
        {
            _grid = GetTemplateChild("RootGrid") as Grid;
            InitRowsAndColumns();
        }

        private void Border_ManipulationDeltaColumn(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var a = _grid.Children.Where(x => x is Path);
            var b = a.Last().TransformToVisual(_grid.Parent as UIElement);
            var c = b.TransformPoint(new Point());
            if (sender is Border border)
            {
                var column = Grid.GetColumn(border);
                if (column == 1)
                {
                    column++;
                    _grid.ColumnDefinitions[column].Width = new GridLength(_grid.ColumnDefinitions[column].Width.Value - e.Delta.Translation.X, GridUnitType.Pixel);
                    Canvas.SetLeft(_grid, Canvas.GetLeft(_grid) + e.Delta.Translation.X);
                }
                else if (column == ColumnCount * 2 + 1)
                {
                    column--;
                    _grid.ColumnDefinitions[column].Width = new GridLength(_grid.ColumnDefinitions[column].Width.Value + e.Delta.Translation.X, GridUnitType.Pixel);
                }
                else
                {
                    _grid.ColumnDefinitions[column + 1].Width = new GridLength(_grid.ColumnDefinitions[column + 1].Width.Value - e.Delta.Translation.X, GridUnitType.Pixel);
                    _grid.ColumnDefinitions[column - 1].Width = new GridLength(_grid.ColumnDefinitions[column - 1].Width.Value + e.Delta.Translation.X, GridUnitType.Pixel);
                }
            }
        }

        private void Border_PointerEnteredColumn(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                var column = Grid.GetColumn(border);
                var button = new Button
                {
                    Content = "+",
                    Width = 23,
                    Height = 23,
                    Margin = new Thickness(-15, -15, -15, -15),
                    CornerRadius = new CornerRadius(25),
                };
                button.PointerEntered += Button_PointerEntered;
                button.PointerExited += Button_PointerExited;
                button.Click += AddColumnButtonClick;
                _grid.Children.Add(button);
                Grid.SetColumn(button, column);
                Grid.SetRow(button, 0);
            }
        }


        private void Border_ManipulationDeltaRow(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                var row = Grid.GetRow(border);
                if (row == 1)
                {
                    row++;
                    _grid.RowDefinitions[row].Height = new GridLength(_grid.RowDefinitions[row].Height.Value - e.Delta.Translation.Y, GridUnitType.Pixel);
                    Canvas.SetTop(_grid, Canvas.GetTop(_grid) + e.Delta.Translation.Y);
                }
                else if (row == RowCount * 2 + 1)
                {
                    row--;
                    _grid.RowDefinitions[row].Height = new GridLength(_grid.RowDefinitions[row].Height.Value + e.Delta.Translation.Y, GridUnitType.Pixel);
                }
                else
                {
                    _grid.RowDefinitions[row + 1].Height = new GridLength(_grid.RowDefinitions[row + 1].Height.Value - e.Delta.Translation.Y, GridUnitType.Pixel);
                    _grid.RowDefinitions[row - 1].Height = new GridLength(_grid.RowDefinitions[row - 1].Height.Value + e.Delta.Translation.Y, GridUnitType.Pixel);
                }
            }
        }

        private void Border_PointerEnteredRow(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                var row = Grid.GetRow(border);
                var button = new Button
                {
                    Content = "+",
                    Width = 23,
                    Height = 23,
                    Margin = new Thickness(-15, -15, -15, -15),
                    CornerRadius = new CornerRadius(25),
                };
                button.PointerEntered += Button_PointerEntered;
                button.PointerExited += Button_PointerExited;
                button.Click += AddRowButtonClick;
                _grid.Children.Add(button);
                Grid.SetColumn(button, 0);
                Grid.SetRow(button, row);
            }
        }

        bool isPointerOnButton = false;

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            isPointerOnButton = false;
            var buttons = _grid.Children.Where(x => x is Button);
            foreach (var item in buttons)
            {
                _grid.Children.Remove(item);
            }
        }

        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            isPointerOnButton = true;
        }

        private void Border_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (isPointerOnButton) return;
            var button = VisualTreeHelper.FindElementsInHostCoordinates(e.GetCurrentPoint(Window.Current.Content).Position, _grid).FirstOrDefault(x => x is Button);
            if (button != null) return;
            var buttons = _grid.Children.Where(x => x is Button);
            foreach (var item in buttons)
            {
                _grid.Children.Remove(item);
            }
        }

        private void AddRowButtonClick(object sender, RoutedEventArgs e)
        {
            fromUI = true;
            RowCount++;
            fromUI = false;
            if (sender is FrameworkElement element)
            {
                InsertRow(Grid.GetRow(element));
            }
        }

        private void AddColumnButtonClick(object sender, RoutedEventArgs e)
        {
            fromUI = true;
            ColumnCount++;
            fromUI = false;
            if (sender is FrameworkElement element)
            {
                InsertColumn(Grid.GetColumn(element));
            }
        }
        private void DeleteRow(object sender)
        {
            var callingButton = (Button)sender;
            int rowNumber = Grid.GetRow(callingButton);
            int callingButtonIndex = _grid.Children.IndexOf(callingButton);
            foreach (var child in _grid.Children.ToArray())
            {
                var childRow = (int)child.GetValue(Grid.RowProperty);
                if (childRow == rowNumber)
                {
                    //this child should be removed
                    _grid.Children.Remove(child);
                }
                else if (childRow > rowNumber)
                {
                    //move items on next rows one row up
                    child.SetValue(Grid.RowProperty, childRow - 1);
                }
            }
            _grid.RowDefinitions.RemoveAt(rowNumber);
            // Grid.SetRow(btnAddSource, _grid.RowDefinitions.Count - 1);
        }

        private void InsertRow(int index)
        {
            _grid.RowDefinitions.Insert(index, new RowDefinition()
            {
                Height = new GridLength(ItemHeight, GridUnitType.Pixel),
            });
            _grid.RowDefinitions.Insert(index, new RowDefinition()
            {
                Height = new GridLength(3, GridUnitType.Pixel),
            });
            foreach (var child in _grid.Children.ToArray())
            {
                var childRow = (int)child.GetValue(Grid.RowProperty);
                if (childRow >= index)
                    child.SetValue(Grid.RowProperty, childRow + 2);
            }

            var border = new Border
            {
                Name = "RowBorder",
                BorderThickness = new Thickness(3),
                Height = 3,
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255)),
                ManipulationMode = ManipulationModes.TranslateY
            };
            border.PointerEntered += Border_PointerEnteredRow;
            border.PointerExited += Border_PointerExited;
            border.ManipulationDelta += Border_ManipulationDeltaRow;
            _grid.Children.Add(border);
            Grid.SetColumn(border, 1);
            Grid.SetColumnSpan(border, ColumnCount * 2 + 1);
            border.SetValue(Grid.RowProperty, index);
            var columnBorders = _grid.Children.Where(x =>
            {
                if (x is FrameworkElement frameworkElement)
                    return frameworkElement.Name == "ColumnBorder";
                return false;
            });
            foreach (var item in columnBorders)
            {
                if (Convert.ToInt32(item.GetValue(Grid.ColumnProperty)) % 2 == 1)
                {
                    item.SetValue(Grid.RowProperty, 1);
                    item.SetValue(Grid.RowSpanProperty, RowCount * 2 + 1);
                }
            }
        }

        private void InsertColumn(int index)
        {
            _grid.ColumnDefinitions.Insert(index, new ColumnDefinition()
            {
                Width = new GridLength(ItemWidth, GridUnitType.Pixel),
            });
            _grid.ColumnDefinitions.Insert(index, new ColumnDefinition()
            {
                Width = new GridLength(3, GridUnitType.Pixel),
            });
            foreach (var child in _grid.Children.ToArray())
            {
                var childColumn = (int)child.GetValue(Grid.ColumnProperty);
                if (childColumn >= index)
                    child.SetValue(Grid.ColumnProperty, childColumn + 2);
            }

            var border = new Border
            {
                Name = "ColumnBorder",
                BorderThickness = new Thickness(3),
                Width = 3,
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255)),
                ManipulationMode = ManipulationModes.TranslateX
            };
            border.PointerEntered += Border_PointerEnteredColumn;
            border.PointerExited += Border_PointerExited;
            border.ManipulationDelta += Border_ManipulationDeltaColumn;
            _grid.Children.Add(border);
            Grid.SetRow(border, 1);
            Grid.SetRowSpan(border, RowCount * 2 + 1);
            border.SetValue(Grid.ColumnProperty, index);
            var rowBorders = _grid.Children.Where(x =>
            {
                if (x is FrameworkElement frameworkElement)
                    return frameworkElement.Name == "RowBorder";
                return false;
            });
            foreach (var item in rowBorders)
            {
                if (Convert.ToInt32(item.GetValue(Grid.RowProperty)) % 2 == 1)
                {
                    item.SetValue(Grid.ColumnProperty, 1);
                    item.SetValue(Grid.ColumnSpanProperty, ColumnCount * 2 + 1);
                }
            }

        }
    }
}
