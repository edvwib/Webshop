document.querySelectorAll('.productPrice').forEach(priceBtn => {
  priceBtn.addEventListener('mouseover', (e) => {
    e.target.value = 'Add to cart';
  });
  priceBtn.addEventListener('mouseout', (e) => {
    e.target.value = e.target.dataset.productprice;
  });
});
