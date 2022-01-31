package com.example.cardreader;

public class ListItems {
    String name;
    String title;
    String phone;
    String email;
    String id;

    public ListItems(String name, String title, String phone, String email) {
        this.name = name;
        this.title = title;
        this.phone = phone;
        this.email = email;
    }

    public ListItems(String name, String title, String phone, String email, String id) {
        this.name = name;
        this.title = title;
        this.phone = phone;
        this.email = email;
        this.id = id;
    }

    public String getName() {
        return this.name;
    }

    public String getTitle() {
        String[] title = {"Manager", "Assistant", "Accountant"};
        for (int i = 0; i < title.length; i++) {
            if (this.title == title[i]){
                this.title = title[i];
            }
        }
        return this.title;
    }

    public String getPhone() {
//        long num = 0410000000;
//        if (this.phone < num)
//            this.phone = num;
        return this.phone;
    }

    public String getEmail() {
//        String str = " ";
//        if (str.indexOf("@") != -1)
//            this.email = str;
        return this.email;
    }

    public String getId() {
        return this.id;
    }

    public void setId(String id) {
        this.id = id;
    }

    @Override
    public String toString(){
        return name + "\n" + title + "\n" + phone + "\n" + email;
    }

}
